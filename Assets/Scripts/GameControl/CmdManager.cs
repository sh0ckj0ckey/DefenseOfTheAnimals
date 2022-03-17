using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CmdManager : MonoBehaviour
{
    private const string CHEATCODE_GIVEMEMONEY = "GIVEMEMONEY";
    private const string CHEATCODE_WHOSYOURDADDY = "WHOSYOURDADDY";

    public InputField CmdTextBox;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CmdTextBox.isFocused)
            {
                DealWithCommand(CmdTextBox.text);
                CmdTextBox.text = "";
                CmdTextBox.DeactivateInputField();
            }
            else
            {
                CmdTextBox.text = "";
                CmdTextBox.ActivateInputField();
            }
        }
    }

    public void OnEditEnd()
    {
        CmdTextBox.text = "";
    }

    private void DealWithCommand(string cmd)
    {
        if (cmd.ToUpper() == CHEATCODE_GIVEMEMONEY)
        {
            BuildManager.Instance.GiveMeMoney(9999);
        }
        else if (cmd.ToUpper() == CHEATCODE_WHOSYOURDADDY)
        {

        }
    }
}
