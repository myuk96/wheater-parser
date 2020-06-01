using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CrawlingSystem.Crawler.Weather.Extensions
{
    public static class LinqDataFlowExtensions
    {
        public static IPropagatorBlock<TInput, TOutput> SelectMany<TInput, TOutput>(this ISourceBlock<TInput> source,
            Func<TInput, IEnumerable<TOutput>> transform)
        {
            var transformBlock = new TransformManyBlock<TInput, TOutput>(transform);
            source.LinkTo(transformBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return transformBlock;
        }

        public static IPropagatorBlock<TInput, TOutput> Select<TInput, TOutput>(this ISourceBlock<TInput> source,
            Func<TInput, Task<TOutput>> transform)
        {
            var transformBlock = new TransformBlock<TInput, TOutput>(transform, new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount + 1
            });
            source.LinkTo(transformBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return transformBlock;
        }

        public static ITargetBlock<TInput> ForEach<TInput>(this ISourceBlock<TInput> source,
            Func<TInput, Task> action)
        {
            var actionBlock = new ActionBlock<TInput>(action, new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount + 1
            });
            source.LinkTo(actionBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return actionBlock;
        }

        public static IPropagatorBlock<TInput, TOutput> Select<TInput, TOutput>(this ISourceBlock<TInput> source,
            Func<TInput, TOutput> transform)
        {
            var transformBlock = new TransformBlock<TInput, TOutput>(transform);
            source.LinkTo(transformBlock, new DataflowLinkOptions { PropagateCompletion = true });

            return transformBlock;
        }
    }
}
