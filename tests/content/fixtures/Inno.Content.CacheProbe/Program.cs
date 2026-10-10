using System;
using System.IO;
using System.Threading.Tasks;
using Inno.Adapter.Content.FileSystem;
using Inno.Content;
using Inno.Content.Testing;

internal static class Program
{
    private static async Task<int> Main(string[] arguments)
    {
        try
        {
            using ContentTestStore source = ContentTestStore.FromDirectory(arguments[0]);
            Console.WriteLine("ready");
            if (await Console.In.ReadLineAsync() != "prepare")
                return 2;
            using FileContentStore store = await FileContentPreparation.PrepareAsync(source,
                new FileContentCacheOptions(arguments[1]));
            using ContentReadLease lease = store.Acquire(new ContentKey("pinned.bin"));
            using Stream reader = lease.OpenRead();
            string cache = Path.Combine(arguments[1], "Content", source.descriptor.contentHash);
            Console.WriteLine(File.ReadAllText(Path.Combine(cache, "current")));
            if (await Console.In.ReadLineAsync() != "release")
                return 3;
            byte[] bytes = new byte[checked((int)lease.entry.length)];
            reader.ReadExactly(bytes);
            Console.WriteLine(Convert.ToHexString(bytes));
            return 0;
        }
        catch (Exception failure)
        {
            Console.Error.WriteLine(failure);
            return 1;
        }
    }
}
