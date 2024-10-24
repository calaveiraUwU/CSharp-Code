using Cinemachine;
using StarterAssets;
using UnityEngine;

public class StopPlayerMovement : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fpc;
    [SerializeField] private CinemachineBrain _cmb;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private StarterAssetsInputs _inputs;
    public static StopPlayerMovement Instance { get; private set; }


    private bool _playerIsStopped = false;

    private void Awake()
    {
        // Verificamos si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Esto asegura que no se destruya al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Si ya existe, destruimos este objeto
        }

        _fpc = FindFirstObjectByType<FirstPersonController>();
        _cmb = FindAnyObjectByType<CinemachineBrain>();
        _cam = FindAnyObjectByType<CinemachineVirtualCamera>();
        _inputs = FindAnyObjectByType<StarterAssetsInputs>();
    }

    public void UICursorEnable()
    {
        _inputs.cursorInputForLook = false;
        _inputs.cursorLocked = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UICursorDisable()
    {
        _inputs.cursorInputForLook = true;
        _inputs.cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PlayerStop()
    {
        _fpc.enabled = _playerIsStopped;
        _cmb.enabled = _playerIsStopped;
        _cam.enabled = _playerIsStopped;
        _playerIsStopped = !_playerIsStopped;
    }
}
