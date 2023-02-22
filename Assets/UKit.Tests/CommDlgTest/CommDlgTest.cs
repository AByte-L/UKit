using System.IO;
using UnityEngine;
using AByte.UKit.Externs;

public class CommDlgTest : MonoBehaviour
{
   [SerializeField] UnityEngine.UI.Text openfilePathText;

    public void OnSaveFile()
    {
        //打开 StreamingAssets 下的 测试.txt 文件
        string fileName = "测试.txt";
        string sourcePath = Path.Combine(Application.streamingAssetsPath,fileName);
        OFN_Dialog.SaveFile(sourcePath,OFN_Filter.TXT,fileName);

    }

    public void OnOpenFile()
    {

        openfilePathText .text = OFN_Dialog.OpenFile(OFN_Filter.TXT) ;

    }
}
