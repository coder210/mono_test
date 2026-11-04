using Livnet;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Livnet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MyData
    {
        public int Id { get; set; }
        public float Value { get; set; }

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        private string name;

        public string Name { get { return name; } set { name = value; } }
    }

    public static class Log
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void Info(string message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void Info2(out string message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static IntPtr GetData( );

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void PrintData2(IntPtr mydata);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void PrintData(MyData mydata);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void Callback(Action<int, string, IntPtr> action);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal extern static void SetPointer(IntPtr ptr);
    }

    public static class MyClass
    {
        public static void ProcessData(MyData data)
        {
            Console.WriteLine($"Processed: {data.Id}, {data.Value}, {data.Name}");
            //Console.WriteLine($"Processed: {data.Id}, {data.Value}");
        }

        public static string ProcessData(int id, string value)
        {
            Console.WriteLine($"Processed: {id}, {value}");
            return "ProcessData liwei";
        }
    }

}

namespace Test
{
    public class Class1
    {
        public static void Fun()
        {
            Console.WriteLine("Hello World");

            try
            {
                int i = 0;
                int result = 100 / i;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("Hello World");
            List<string> names = new List<string>() {
                "liwei",
                "水利"
            };

            Queue<string> tq = new Queue<string>();
            tq.Enqueue("ok");


            string str = "from c#";
            Log.Info(str);
            Log.Info2(out str);
            Log.Info(str);

            Log.PrintData(new MyData()
            {
                Id = 19,
                Value = 20.2f,
            });

            Console.WriteLine("================================");

            IntPtr t2 = Log.GetData();
            Log.PrintData2(t2);

            foreach (string name in names)
            {
                Console.WriteLine(name);
            }

            Log.Callback((id, name, pointer) => {
                Console.WriteLine("Callback:" + id + "," + name);
                Console.WriteLine("C Pointer:" + pointer);

                int value = Marshal.ReadInt32(pointer);
                Console.WriteLine($"C#: 原始值: {value}");

                int newValue = value * 2;
                Marshal.WriteInt32(pointer, newValue);

                Log.SetPointer(pointer);
            });

        }
    }
}
