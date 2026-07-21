using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChatScript : NetworkBehaviour
{
	Text TxtTexto;
	InputField inputField;

	void Start () 
	{
		TxtTexto = GameObject.Find ("TxtTexto").GetComponent < Text>();
		inputField = GameObject.Find ("input").GetComponent<InputField> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isLocalPlayer)
			return;


		if(Input.GetKeyDown(KeyCode.Return))
		{
			if(inputField.text != "")
			{
				string Mensaje = inputField.text;
				inputField.text = "";

				CmdEnviar (Mensaje);
			}
		}
	}

	[Command]
	void CmdEnviar(string mensaje)
	{
		RpcRecivir (mensaje);

	}

	[ClientRpc]
	public void RpcRecivir(string mensaje)
	{
		TxtTexto.text += ">>" + mensaje + "\n";
	}


}
