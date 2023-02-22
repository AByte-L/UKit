using System;
using System.Runtime.InteropServices;

namespace AByte.UKit.Externs
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        /// <summary>
        /// 指定这个结构的大小，以字节为单位。
        /// </summary>
        public int structSize = 0;
        /// <summary>
        /// 指向所有者对话框窗口的句柄。这个成员可以是任意有效窗口句柄，或如果对话框没有所有者它可以为NULL。
        /// </summary>
        public IntPtr dlgOwner = IntPtr.Zero;
        /// <summary>
        /// 如果在Flags成员中设置了OFN_ENABLETEMPLATEHANDLE标记，hInstance成员指向包含一个对话框模板的内存对象。
        /// 如果OFN_ENABLETEMPLATE标记被设置，hInstance是一个指向通过lpTemplateName成员命名的对话框模板的模块。
        /// 如果两者都没有被设置，这个成员被忽略。
        /// </summary>
        public IntPtr instance = IntPtr.Zero;
        /// <summary>
        /// 指向一对以空字符结束的过滤字符串的一个缓冲。缓冲中的最后一个字符串必须以两个NULL字符结束。
        /// 第一个字符串是过滤器描述的显示字符串（例如，“文本文件”），第二个字符指定过滤样式（例如，“.TXT”）。
        /// 要为一个显示字符串指定多个过滤样式，使用分号（“;”）分隔样式（例如，“.TXT;.DOC;.BAK”）。
        /// 一个样式字符串中可以包含有效的文件名字字符及星号（*）通配符。不能在样式字符串中包含空格。
        /// 系统不能改变过滤器的次序。它按lpstrFilter指定的次序显示在文件类型组合框中。
        /// 如果lpstrFilter是NULL，对话框不能显示任何过滤器。
        /// </summary>
        public String filter = null;
        /// <summary>
        /// 指向一个静态缓冲，它包含了一对以空字符结束的过滤器字符串，这个字符串是为了保留用户选择的过滤样式。
        /// 第一个字符串是描述定制过滤器的显示字符串，第二个字符串是被用户选择的过滤器样式。
        /// 第一次你的应用程序建立对话框，你指定的第一个字符串可以是任何非空的字符串。当用户选择了一个文件时，对话框复制当前过滤样式到第二个字符串。
        /// 保留过滤样式可以是在lpstrFilter缓冲中指定的样式之一，或是用户输入的过滤器样式。
        /// 在下一次对话框被建立时系统使用这个字符串去初始化用户自定义的文件过滤器。如果nFilterIndex成员是0，对话框使用定制过滤器。
        /// 如果这个成员是NULL，对话框不能保留用户自定义过滤器样式。
        /// 如果这个成员不是NULL，nMaxCustFilter成员的值必须指定以TCHARs为单位的lpstrCustomFilter缓冲的大小。
        /// 对于ANSI版本，是字节的个数；对于Unicode版本，是字符的个数。
        /// </summary>
        public String customFilter = null;
        /// <summary>
        /// 指定特意为lpstrCustomFilter准备的以TCHARs为单位的缓冲大小。
        /// 对于ANSI版本，是字节的个数；
        /// 对于Unicode版本，是字符的个数。
        /// 这缓冲应该最小在40个字符长。
        /// 如果lpstrCustomFilter成员是NULL或是指向NULL的字符串，这个成员被忽略。
        /// </summary>
        public int maxCustFilter = 0;
        /// <summary>
        /// 指定在文件类型控件中当前选择的过滤器的索引。
        /// 缓冲指向被lpstrFilter包含的一对定义了的过滤器的字符串。
        /// 过滤器的第一对字符串的索引值为1，第二对为2，等等。
        /// 0索引指出是通过lpstrCustomFilter指定的定制过滤器。
        /// 你可以为对话框指定一个索引作为最初的过滤器描述及过滤器样式。
        /// 当用户选择了一个文件时，nFilterIndex返回当前显示的过滤器的索引。
        /// 如果nFilterIndex是0及lpstrCustomFilter是NULL，系统使用在lpstrFilter缓冲中的第一个过滤器。
        /// 如果所有的三个成员都是0或NULL，系统不使用任何过滤器，在对话框的列表文件中不显示任何文件。
        /// </summary>
        public int filterIndex = 0;
        /// <summary>
        /// 如果是 打开选择文件或者保存时不需要设置默认文件名称 file = new string(new char[256]);
        /// 如果是 保存且需要设置默认名称，将默认名称Copyto到缓冲区中（详见代码实现）
        /// 默认名称如: "damo.pdf" ,这个后缀要不要都行，不要的话，请设置 defExt = "pdf",会自动添加上扩展,且在文件名中也会显示扩展
        /// </summary>
        public String file = null;
        /// <summary>
        /// 这个就设置为256 
        /// </summary>
        public int maxFile = 256;
        /// <summary>
        /// 指向接收选择的文件的文件名和扩展名的缓冲（不带路径信息）。这个成员可以是NULL。
        /// </summary>
        public String fileTitle = null;
        /// <summary>
        /// 指定lpstrFileTitle缓冲的大小，以TCHARs为单位。
        /// 对于ANSI版本，是字节的个数；对于Unicode版本，是字符的个数。
        /// 如果lpstrFileTitle是NULL，这个成员被忽略。
        /// </summary>
        public int maxFileTitle = 64;
        /// <summary>
        /// 指向以空字符结束的字符串，可以在这个字符串中指定初始目录。
        /// Pointer to a null terminated string that can specify the initial directory. 
        /// 在不同的平台上，为选择初始目录有不同的运算法则。
        /// </summary>
        public String initialDir = null;
        /// <summary>
        /// 指向在对话框的标题栏中放置的字符串。如果这个成员是NULL，系统使用默认标题（另存为或打开）
        /// </summary>
        public String title = null;
        /// <summary>
        /// 位标记的设置，你可以使用来初始化对话框。
        /// 当对话框返回时，它设置的这些标记指出用户的输入。
        /// </summary>
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        /// <summary>
        /// 指向包含默认扩展名的缓冲。如果用户忘记输入扩展名，GetOpenFileName和GetSaveFileName附加这个扩展名到文件名中。
        /// 这个字符串可以是任一长度，但只有头三个字符被附加。字符串不应该包含一个句点（.）。
        /// 如果这个成员是NULL并且用户忘记了输入一个扩展名，那么将没有扩展名被附加。
        /// </summary>
        public String defExt = null;
        /// <summary>
        /// 默认扩展，作用是，如果你保存文件时，文件名没加上扩展，会自动给你加上，这里的扩展不包含（.）,正确格式如："pdf"
        /// 好像说是只识别3个字符，未验证
        /// </summary>
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        /// <summary>
        /// 指向一个以空字符结束的字符串，字符串是对话框模板资源的名字，资源保存在能被hInstance成员识别的模块中。
        /// 对于有限的对话框资源，这可以是通过MAKEINTRESOURCE返回的值。
        /// 除非在Flags成员中设置了OFN_ENABLETEMPLATE标记，要么这个成员被忽略。
        /// </summary>
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
}