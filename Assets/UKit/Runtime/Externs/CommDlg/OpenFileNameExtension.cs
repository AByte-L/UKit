using System;
using System.Runtime.InteropServices;

namespace AByte.UKit.Externs
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    /// <summary>
    /// OpenFileName 的扩展设置 默认文件名，在保存时使用
    /// </summary>
    public static class OpenFileNameExtension
    {
        /// <summary>
        /// 设置默认名
        /// </summary>
        /// <param name="ofn"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static void SetDefultFileName(this OpenFileName ofn, string fileName)
        {
            char[] charArr = new char[256];
            fileName.CopyTo(0, charArr, 0, fileName.Length);
            ofn.file = new String(charArr);
            ofn.maxFile = ofn.file.Length;

        }

    }
}