//using static StupidTemplate.Classes.Better_II_temp.SimpleInputs;
namespace StupidTemplate.Classes.Better_II_temp
{
    //Simple inputs takes away the need to use ControllerInputPoller. instead you can just refrence it in a class and use it like if(Righttrigger) an example of how it should be implemented is above
    
    internal class SimpleInputs
    {
        #region controller inputs
        // Right controller
        public static bool RightTrigger => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
        public static bool RightGrab => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightGrab;
        public static bool RightA => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerPrimaryButton;
        public static bool RightB => ControllerInputPoller.instance != null && ControllerInputPoller.instance.rightControllerSecondaryButton;
        //Left Controller
        public static bool LeftTrigger => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
        public static bool LeftGrab => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftGrab;
        public static bool LeftX => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerPrimaryButton;
        public static bool LeftY => ControllerInputPoller.instance != null && ControllerInputPoller.instance.leftControllerSecondaryButton;
        #endregion
    }
}
