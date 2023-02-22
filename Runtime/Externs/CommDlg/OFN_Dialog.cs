
using System.IO;
using System.Runtime.InteropServices;
namespace AByte.UKit.Externs
{
    public class OFN_Dialog
    {
        //链接指定系统函数       打开文件对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
        //链接指定系统函数        另存为对话框
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

        private static string GetFilter(OFN_Filter filter)
        {
            switch (filter)
            {
                case OFN_Filter.TXT:
                    return "文本文件(*.txt)\0*.txt";
                case OFN_Filter.PDF:
                    return "PDF文件(*.pdf)\0*.pdf";
                case OFN_Filter.MP4:
                    return "MP4文件(*.mp4)\0*.mp4";

            }
            return "*.*";

        }
        /// <summary>
        /// 保存路径
        /// </summary>
        /// <param name="filePath">源文件路径，包含扩展</param>
        /// <param name="filter">如：“PDF文件(*.pdf)\0*.pdf”</param>
        /// <param name="defExt">默认扩展，如："pdf"</param>
        /// <param name="defFielname">保存的文件名，如:"pdf测试"</param>
        public static bool SaveFile(string filePath, string filter, string defExt, string defFielname = "")
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = filter;//
            openFileName.SetDefultFileName(defFielname);
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.title = "保存";//窗口标题
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008 | (int)OFN_Flags.OFN_OVERWRITEPROMPT;
            openFileName.defExt = defExt;

            if (GetSaveFileName(openFileName))
            {
                UnityEngine.Debug.Log(openFileName.file);
                //判断目标路径是否存在相同的文件，有就删除
                if (File.Exists(openFileName.file))
                {
                    File.Delete(openFileName.file);
                }

                string savePath = openFileName.file.Replace("\\", "/");
                UnityEngine.Debug.Log("保存路径： " + savePath);
                try
                {
                    File.Copy(filePath, savePath);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogWarning("Exception " + e.Message);
                }
                return true;
            }
            return false;

        }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="filter">文件类型</param>
        /// <param name="defFielname">保存文件的默认名称如: demo,会自动加上扩展名</param>
        public static bool SaveFile(string filePath, OFN_Filter filter, string defFielname = "")
        {
            return SaveFile(filePath, GetFilter(filter), filter.ToString().ToLower(), defFielname);
        }

        /// <summary>
        /// 打开对话框选择文件，并返回路径
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        public static string OpenFile(OFN_Filter filter)
        {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = GetFilter(filter);//
            openFileName.file = new string(new char[256]);
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.title = "选择文件";//窗口标题
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            openFileName.defExt = filter.ToString().ToLower();

            if (GetOpenFileName(openFileName))
            {
                return openFileName.file;
            }
            return null;
        }
    }

}
