using UnityEngine;
using UnityEngine.UI;

namespace PandoraCube
{
    public class PandoraCube : MonoBehaviour
    {
        // Target FPS
        [Tooltip("Sets the Application's targetFrameRate")]
        public int tfps = 60;

        // Debug gizmo.
        public GameObject axisGizmo;

        private void Awake()
        {
            Debug.Log("PandoraCube: Awake() called");

            // Disable the gizmo so we don't get an artifact at origin.
            if (axisGizmo)
            {
                axisGizmo.SetActive(false);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // TODO Left here for reference.
            //PlayerInput pi = GetComponent<PlayerInput>();
            //pi.actions["UI/Click"].performed += OnClick;
            // InputSystemUIInputModule uii = GameObject.Find("EventSystem").GetComponent<InputSystemUIInputModule>();

            Debug.Log("PandoraCube: Start() called");

            Debug.Log("PandoraCube: Set target FPS to " + tfps);
            Application.targetFrameRate = tfps;

            SetupGUIButtonEventHandlers();
        }

        // ISSUE: When scripts are reloaded/recomplied during Play mode the event system (or something else)
        //        gets bugged and doesn't fire mouse events. These handler setup can be done again when
        //        the assemblies are reloaded (see below). However, something else gets bugged and doesn't
        //        translate mouse clicks to button onClick events. I've spent about 3 hours wasting time
        //        on this bug that doesn't appear if there is no asset raloading.
        protected void SetupGUIButtonEventHandlers()
        {

            // TODO Remove/refactor when the UI elements are either removed or overhauled.
            // Setup UI controls callbacks.
            Cube cube = GameObject.Find("the-cube").GetComponent<Cube>();
            GameObject.Find("Button Up")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Rotate(Vector2.up); });
            GameObject.Find("Button Right")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Rotate(Vector2.right); });
            GameObject.Find("Button Down")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Rotate(Vector2.down); });
            GameObject.Find("Button Left")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Rotate(Vector2.left); });
            GameObject.Find("Button Action")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Activate(); });
            GameObject.Find("Button Reset")
                .GetComponent<Button>().onClick.AddListener(() => { cube.OnPlayerAction_Reset(); });
        }

        /**
         * Creates a debug gizmo that displays axis alignment.
         */
        public GameObject CreateAxisGizmo()
        {
            if (axisGizmo)
            {
                GameObject ag = Instantiate(axisGizmo, Vector3.zero, Quaternion.identity);
                // The prototype is disabled, so make sure it's enabled when passing it
                // to the user.
                ag.SetActive(true);
                return ag;
            }
            else
            {
                Debug.Log("PandoraCube: No axis gizmo.");
            }
            return null;
        }

        //protected void OnEnable() 
        //{
        //    Debug.Log("PandoraCube: OnEnable()");
        //    AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        //}

        //protected void OnDisable()
        //{
        //    Debug.Log("PandoraCube: OnDisable()");
        //    AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        //}

        //protected void OnAfterAssemblyReload()
        //{
        //    Debug.Log("PandoraCube: OnAfterAssemblyReload()");   
        //    Debug.Log("PandoraCube: Setup GUI button handlers");      
        //    SetupGUIButtonEventHandlers();
        //}
    }
}