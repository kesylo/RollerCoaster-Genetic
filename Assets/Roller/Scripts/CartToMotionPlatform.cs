using MotionSystems;
using System.Runtime.InteropServices;
using UnityEngine;

public class CartToMotionPlatform : MonoBehaviour
{
    // CART
    public GameObject m_runningCart;

    //Heave maximum value that is available in the game
    private const float DRAWING_HEAVE_MAX = 1.0f;

    //Heave change step
    private const float DRAWING_HEAVE_STEP = 0.05f;

    //Maximum value of pitch angle that is avialable in the game
    private const float DRAWING_PITCH_MAX = 16.0F;

    //pITCH change step
    private const float DRAWING_PITCH_STEP = 1.0f;

    //Maximum value of roll angle that is avialable in the game
    private const float DRAWING_ROLL_MAX = 16.0F;

    //pITCH change step
    private const float DRAWING_ROLL_STEP = 1.0f;

    //Current platform's heave in degree in game
    public float m_heave = 0;

    //Current platform's pitch in degree in game
    public float m_pitch = 0;

    //Current platform's angle in degree in the game
    public float m_roll = 0;

    //FSMI api
    private ForceSeatMI m_fsmi;

    // Position in physical coordinates that will be send to the platform
    private FSMI_TopTablePositionPhysical m_platformPosition = new FSMI_TopTablePositionPhysical();

    // Use this for initialization
    void Start()
    {
        //Load ForceSeatMI Library from ForceSeatPM istallation directory
        m_fsmi = new ForceSeatMI();

        //Find Running cart object
        //m_runningCart = GameObject.Find("Running_Cart");

        if (m_fsmi.IsLoaded())
        {
            //Prepare data structure by clearing it and setting correct size
            m_platformPosition.mask = 0;
            m_platformPosition.structSize = (byte)Marshal.SizeOf(m_platformPosition);

            m_platformPosition.state = FSMI_State.NO_PAUSE;

            //Set fields that can be changed by demo application
            m_platformPosition.mask = FSMI_POS_BIT.STATE | FSMI_POS_BIT.POSITION;

            m_fsmi.BeginMotionControl();
        }
        else
        {
            Debug.LogError("ForceSeatMI Library has not been found! Please install ForceSeatPM.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float angle = m_runningCart.transform.eulerAngles.x;
        if (angle > 180f) angle -= 360f;
        m_pitch = angle / 4f;
        //Debug.Log("x = " + m_runningCart.transform.eulerAngles.x);
        //Debug.Log("m_pitch = " + m_pitch);

        angle = m_runningCart.transform.eulerAngles.z;
        if (angle > 180f) angle -= 360f;
        //m_roll = angle / 5f;
        Debug.Log("z = " + m_runningCart.transform.eulerAngles.z);
        Debug.Log("m_roll = " + m_roll);

        //Update values in order to receive cart position/rotation inputs
        if (m_fsmi.IsLoaded())
        {
            if (m_pitch > 0)
            {
                m_pitch = Mathf.Clamp(m_pitch - DRAWING_PITCH_STEP, 0, DRAWING_PITCH_MAX);
            }
            else if (m_pitch < 0)
            {
                m_pitch = Mathf.Clamp(m_pitch + DRAWING_PITCH_STEP, -DRAWING_PITCH_MAX, 0);
            }

            if (m_roll > 0)
            {
                m_roll = Mathf.Clamp(m_roll - DRAWING_ROLL_STEP, 0, DRAWING_ROLL_MAX);
            }
            else if (m_roll < 0)
            {
                m_roll = Mathf.Clamp(m_roll + DRAWING_ROLL_STEP, -DRAWING_ROLL_MAX, 0);
            }

            if (m_heave > 0)
            {
                m_heave = Mathf.Clamp(m_heave - DRAWING_HEAVE_STEP, 0, DRAWING_HEAVE_MAX);
            }
            else if (m_roll < 0)
            {
                m_roll = Mathf.Clamp(m_heave + DRAWING_HEAVE_STEP, -DRAWING_HEAVE_MAX, 0);
            }

            SendDataToPlatform();
        }
    }

    private void SendDataToPlatform()
    {
        //Convert parameters to logical units
        m_platformPosition.state = FSMI_State.NO_PAUSE;
        m_platformPosition.roll = Mathf.Deg2Rad * m_roll;
        m_platformPosition.pitch = -Mathf.Deg2Rad * m_pitch;
        m_platformPosition.heave = m_heave * 100;

        //Send data to platform
        m_fsmi.SendTopTablePosPhy(ref m_platformPosition);
    }

    void OnDestroy()
    {
        if (m_fsmi.IsLoaded())
        {
            m_fsmi.EndMotionControl();
            m_fsmi.Dispose();
        }
    }
}
