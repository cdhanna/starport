using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smallgroup.Starport.Assets.Core.Generation
{
    class GenerationRunner
    {

        private string[] _requiredTags;

        public GenerationRunner(string[] requiredTags)
        {
            _requiredTags = requiredTags;
        }


        public List<GenerationAction> Run<TCoordinate, TCell, TContext>(TContext global, Map<TCoordinate, TCell> map, Action<TContext, TCoordinate> coordContextCreator,  params GenerationRule<TContext>[] rules)
            where TCoordinate : ICoordinate<TCoordinate>, new()
            where TCell : ICell<TCell>, new()
            where TContext : GenerationContext, new()
        {
            var returnActions = new List<GenerationAction>();

            var rand = new Random();

            var coord2ctx = new Dictionary<TCoordinate, TContext>();

            foreach (var coord in map.Coordinates)
            {
                var coordCtx = new TContext();
                coordCtx.SetParent(global);
                coordContextCreator(coordCtx, coord);

                coord2ctx.Add(coord, coordCtx);
                global.SetSubContext(coord, coordCtx);
            }

            foreach (var coord in map.Coordinates)
            {
                //var coordCtx = new TContext(); // TODO: figure out how to set the data on a coord
                //coordCtx.SetParent(global);
                //coordContextCreator(coordCtx, coord);

                var ruleOutcomes = new Dictionary<string, GenerationRule<TContext>>();
                var ruleScores = new Dictionary<string, int>();
                var coordCtx = coord2ctx[coord];
                foreach (var rule in rules)
                {
                    var outcomes = rule.EvaluateConditions(coordCtx);
                    var ruleScore = outcomes.Count(outcome => outcome == true);
                    if (outcomes.Any(outcome => outcome == false))
                    {
                        ruleScore = 0;
                    }
                    if (ruleScore > 0)
                    {

                        var existingScore = 0;
                        if (ruleScores.TryGetValue(rule.Tag, out existingScore))
                        {
                            if (ruleScore > existingScore || (ruleScore == existingScore && rand.Next() % 2 == 0))
                            {
                                ruleScores[rule.Tag] = ruleScore;
                                ruleOutcomes[rule.Tag] = rule;
                            }
                        }
                        else
                        {
                            ruleScores.Add(rule.Tag, ruleScore);
                            ruleOutcomes.Add(rule.Tag, rule);
                        }
                    }

                }

                var allActions = ruleOutcomes.Values.ToList().Select(rule => rule.Execute(coordCtx));
                //allActions.ToList().ForEach(a => a)
                

                allActions.ToList().ForEach(set => returnActions.AddRange(set));
            }

            return returnActions;
             

            // run rules against context. Sort by category->ordered set of rules
                // for every coord, run a rule in each required category. 


            




        }

    }
}
