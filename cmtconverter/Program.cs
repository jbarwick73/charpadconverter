namespace ctmconverter;
class Program
{

    static void Main(string[] args)
    {
        if (string.IsNullOrEmpty(args[0]))
        {
            Console.WriteLine("Please provide a path to a .asm file.");
            return;
        }

        var ctmfile = args[0];
        var outputpath = args[0].Substring(0, args[0].LastIndexOf("\\"));
        var outputfilename = args[0].Substring(args[0].LastIndexOf("\\")).Replace("\\","").Replace(".asm","");

        if (!File.Exists(ctmfile))
        {
            Console.WriteLine("File does not exist.");
            return;
        }
        var ctmContents = File.ReadAllText(ctmfile);

        var materials = ctmContents.Substring(ctmContents.IndexOf("* = addr_charset_attrib_L1_data")).Replace("* = addr_charset_attrib_L1_data\r\ncharset_attrib_L1_data","");
        materials = ".byte " + materials.Substring(0, materials.IndexOf("; Map Data...")).Replace("\r\n", "").Replace(".byte", "").Replace(" ", "").Replace("$", ",$").Replace(",,$", ",$").Substring(1);
        var charset = ctmContents.Substring(ctmContents.IndexOf("* = addr_charset_data")).Replace("* = addr_charset_data\r\ncharset_data", "");
        charset = ".byte "  +charset.Substring(0, charset.IndexOf("; CharSet Attribute (L1) Data...")).Replace("\r\n", "").Replace(".byte", "").Replace(" ", "").Replace("$", ",$").Replace(",,$", ",$").Substring(1);
        var screen = ".byte " + ctmContents.Substring(ctmContents.IndexOf("* = addr_map_data")).Replace("* = addr_map_data\r\nmap_data", "").Replace("\r\n", "").Replace(".byte", "").Replace(" ", "").Replace("$", ",$").Replace(",,$", ",$").Substring(1);


        if (File.Exists($"{outputpath}\\{outputfilename}_materials.asm"))
            File.Delete($"{outputpath}\\{outputfilename}_materials.asm");
        File.WriteAllText($"{outputpath}\\{outputfilename}_materials.asm", materials);
        
        if (File.Exists($"{outputpath}\\{outputfilename}_screen.asm"))
            File.Delete($"{outputpath}\\{outputfilename}_screen.asm");
        File.WriteAllText($"{outputpath}\\{outputfilename}_screen.asm", screen);

        if (File.Exists($"{outputpath}\\{outputfilename}_charset.asm"))
            File.Delete($"{outputpath}\\{outputfilename}_charset.asm");
        File.WriteAllText($"{outputpath}\\{outputfilename}_charset.asm", charset);
        
    }
}