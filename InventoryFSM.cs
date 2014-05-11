using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

// Base class of all non-player entities
public class InventoryFSM : StateMachine, IXMLParserSetup
{
	#region parameter

	// owner weapon
	public	EntityWeapon	m_Inventory	= null;

	#endregion
	#region FSM_XML_PARSE
	// Init the FSM from a XML node
	// @xmlNodeSettings: the XML node to load
	// @ret:             whether the function succeeds or not
	public virtual bool InitFromXml(XmlNode xmlNodeSettings)
	{
		return false;
	}

	// Add a given state instance to the FSM
	// @stateName:  the AI state to add
	// @ret:        whether the function succeeds or not
	public bool AddStateFromName(string stateName, Type typeinfo )
	{
		InventoryState InvState = null;
		InvState = (InventoryState)Activator.CreateInstance(typeinfo);
		if ( InvState != null )
		{
			if ( false == m_stateList.ContainsKey(stateName) )
			{
				m_stateList.Add( stateName, InvState );
				return true;
			}
		}
		return false;
	}	
	#endregion


	#region FSM_FUNCTION
	public void	SetInventory( EntityWeapon inv )
	{
		m_Inventory = inv;
	}



	public void OnInitialize()
	{
		AddStateFromName( "IS_InActive", typeof(IS_InActive) );
		AddStateFromName( "IS_Active", typeof(IS_Active) );
		AddStateFromName( "IS_Firing", typeof(IS_Firing) );
		AddStateFromName( "IS_Reloading", typeof(IS_Reloading) );
		AddStateFromName( "IS_Equipping", typeof(IS_Equipping) );
		AddStateFromName( "IS_Puttingdown", typeof(IS_Puttingdown) );
		AddStateFromName( "IS_Sprinting", typeof(IS_Sprinting) );
		AddStateFromName( "IS_Hidden", typeof(IS_Hidden) );

		// init states (once taskManager is set and the FSM is ready)
		foreach(State state in m_stateList.Values)
		{
			InventoryState InvState = state as InventoryState;
			if(null != InvState)
			{
				InvState.Init(this);
			}
		}
	}

	public void StartFire( int firenum )
	{
		InventoryState state = m_curState as InventoryState;
		state.StartFire(firenum);
	}

	public void StopFire( int firenum )
	{
		InventoryState state = m_curState as InventoryState;
		state.StopFire(firenum);
	}

	public void ReloadWeapon()
	{
		InventoryState state = m_curState as InventoryState;
		state.ReloadWeapon();
	}


	public void Sprint()
	{
		InventoryState state = m_curState as InventoryState;
		state.Sprint();
	}

	public void SprintOut()
	{
		InventoryState state = m_curState as InventoryState;
		if ( state != null )
			state.SprintOut();
	}

	public void Putdown()
	{
		InventoryState state = m_curState as InventoryState;
		state.Putdown();
	}


	public void HiddenWeapon()
	{
		InventoryState state = m_curState as InventoryState;
		state.HiddenWeapon();
	}


	public void UnHiddenWeapon()
	{
		InventoryState state = m_curState as InventoryState;
		if( state != null )
			state.UnHiddenWeapon();
	}
	#endregion
}
