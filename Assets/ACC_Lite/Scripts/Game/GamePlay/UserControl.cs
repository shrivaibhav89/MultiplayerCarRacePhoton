using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// For user multiplatform control.
/// </summary>
[RequireComponent(typeof(CarController))]
public class UserControl : NetworkBehaviour
{

	CarController ControlledCar;

	public float Horizontal { get; private set; }
	public float Vertical { get; private set; }
	public bool Brake { get; private set; }

	public static MobileControlUI CurrentUIControl { get; set; }

	private void Awake()
	{
		ControlledCar = GetComponent<CarController>();
		CurrentUIControl = FindObjectOfType<MobileControlUI>();
	}

	public override void FixedUpdateNetwork()
	{
		if (GetInput(out NetworkInputData data))
		{
			UpdateCarControll(data);
		}
	}
	void UpdateCarControll(NetworkInputData data)
	{

		if (CurrentUIControl != null && CurrentUIControl.ControlInUse)
		{
			//Mobile control.
			Horizontal = CurrentUIControl.GetHorizontalAxis;
			Vertical = CurrentUIControl.GetVerticalAxis;
		}
		else
		{
			//Standart input control (Keyboard or gamepad).
			Horizontal = data.horizontal;
			Vertical =data.vertical;
			Brake = data.handBreak;
			Debug.Log("Horizontal: " + Horizontal + " Vertical: " + Vertical + " Brake: " + Brake);
		}

		//Apply control for controlled car.
		ControlledCar.UpdateControls(Horizontal, Vertical, Brake);
	}
	// void Update ()
	// {
	// 	if (CurrentUIControl != null && CurrentUIControl.ControlInUse)
	// 	{
	// 		//Mobile control.
	// 		Horizontal = CurrentUIControl.GetHorizontalAxis;
	// 		Vertical = CurrentUIControl.GetVerticalAxis;
	// 	}
	// 	else
	// 	{
	// 		//Standart input control (Keyboard or gamepad).
	// 		Horizontal = Input.GetAxis ("Horizontal");
	// 		Vertical = Input.GetAxis ("Vertical");
	// 		Brake = Input.GetButton ("Jump");
	// 	}

	// 	//Apply control for controlled car.
	// 	ControlledCar.UpdateControls (Horizontal, Vertical, Brake);
	// }
}
