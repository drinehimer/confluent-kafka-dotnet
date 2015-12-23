using System;
using RdKafka;
using System.Linq;

namespace Misc
{
    public class Program
    {
        static string ToString(int[] array) => $"[{string.Join(", ", array)}]";

        public static void Main(string[] args)
        {
            Console.WriteLine($"Hello RdKafka!");
            Console.WriteLine($"{Library.Version:X}");
            Console.WriteLine($"{Library.VersionString}");
            Console.WriteLine($"{string.Join(", ", Library.DebugContexts)}");

            if (args.Contains("-m") || args.Contains("--metadata"))
            {
                using (var producer = new Producer(args[0]))
                {
                    var meta = producer.Metadata();
                    Console.WriteLine($"{meta.OriginatingBrokerId} {meta.OriginatingBrokerName}");
                    meta.Brokers.ForEach(broker =>
                        Console.WriteLine($"Broker: {broker.BrokerId} {broker.Host}:{broker.Port}"));

                    meta.Topics.ForEach(topic =>
                    {
                        Console.WriteLine($"Topic: {topic.Topic} {topic.Error}");
                        topic.Partitions.ForEach(partition =>
                        {
                            Console.WriteLine($"  Partition: {partition.PartitionId}");
                            Console.WriteLine($"    Replicas: {ToString(partition.Replicas)}");
                            Console.WriteLine($"    InSyncReplicas: {ToString(partition.InSyncReplicas)}");
                        });
                    });
                }
            }

            if (args.Contains("-d") || args.Contains("--dump-config"))
            {
                foreach (var kv in new Config().Dump())
                {
                    Console.WriteLine($"\"{kv.Key}\": \"{kv.Value}\"");
                }
            }
        }
    }
}