using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Transform path;
    private Transform ball;
    private Vector3 startMousePos, startBallPos;
    private bool isMoving = false;
    [Range(0f, 1f)] public float maxSpeed;
    [Range(0f, 1f)] public float camSpeed;
    [Range(0f, 50f)] public float pathSpeed;
    private float velocity,camVelocity_x, camVelocity_y;
    private Camera mainCamera;
    private Rigidbody rb;
    private Collider _collider;
    private Renderer BallRender;
    public ParticleSystem collidEffect;
    public ParticleSystem airEffect;
    private void Start()
    {
        mainCamera = Camera.main;
        ball = transform;
        rb = GetComponent<Rigidbody>();
        _collider=GetComponent<Collider>();
        BallRender = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    UpdateBallPosition();
                    break;
                case TouchPhase.Moved:
                    UpdateBallPosition();
                    break;
                case TouchPhase.Stationary:
                    UpdateBallPosition();
                    break;
                case TouchPhase.Ended:
                    ReleaseFinger();
                    break;
                case TouchPhase.Canceled:
                    ReleaseFinger();
                    break;
            }
        }
    }
    private void UpdateBallPosition()
    {
        if(MenuManager.instance.GameState)
        { 
            isMoving = true;
            Plane newPlane = new Plane(Vector3.up, 0f);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(newPlane.Raycast(ray,out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = ball.position;
            }

            if(isMoving)
            {
                Vector3 newMousePos = ray.GetPoint(distance);
                Vector3 NewMousePos = newMousePos - startMousePos;
                Vector3 wantedBallPos = newMousePos + startBallPos;
                wantedBallPos.x = Mathf.Clamp(wantedBallPos.x,-1.5f,1.5f);
                ball.position = new Vector3 (Mathf.SmoothDamp(ball.position.x, wantedBallPos.x, ref velocity, maxSpeed), ball.position.y,ball.position.z);
            }
       
      
          var pathNewPos = path.position;
            path.position = Vector3.MoveTowards(pathNewPos, new Vector3(pathNewPos.x, pathNewPos.y, -1000f), Time.deltaTime * pathSpeed);
          //path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));
        }
            ////get the Vector2 position of the first finger on the screen (touch 0)
        //Vector3 screenTouchPosition = Input.GetTouch(0).position;
        ////translate the screen position to a world position
        //Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenTouchPosition);
        ////put the ball at the WorldPosition location
        //ball.transform.position = worldPosition;
    }
    private void ReleaseFinger()
    {
        isMoving = false;
    }
    private void LateUpdate()
    {
        var CamNewPos = mainCamera.transform.position;
        if(rb.isKinematic)
        mainCamera.transform.position = new Vector3(Mathf.SmoothDamp(CamNewPos.x, ball.transform.position.x, ref camVelocity_x, camSpeed), Mathf.SmoothDamp(CamNewPos.y, ball.transform.position.y +3f, ref camVelocity_y, camSpeed), CamNewPos.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        { gameObject.SetActive(false);
            MenuManager.instance.GameState = false;
        }
        switch(other.tag)
        {
            case "Red":
                other.gameObject.SetActive(false);
                BallRender.material = other.GetComponent<Renderer>().material;
                var newParticle = Instantiate(collidEffect, transform.position, Quaternion.identity);
                newParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Green":
                other.gameObject.SetActive(false);
                BallRender.material = other.GetComponent<Renderer>().material;
                var newParticle1 = Instantiate(collidEffect, transform.position, Quaternion.identity);
                newParticle1.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Blue":
                other.gameObject.SetActive(false);
                BallRender.material = other.GetComponent<Renderer>().material;
                var newParticle2 = Instantiate(collidEffect, transform.position, Quaternion.identity);
                newParticle2.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Yellow":
                other.gameObject.SetActive(false);
                BallRender.material = other.GetComponent<Renderer>().material;
                var newParticle3 = Instantiate(collidEffect, transform.position, Quaternion.identity);
                newParticle3.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("path"))
        {
            rb.isKinematic = _collider.isTrigger = false;
            rb.velocity = new Vector3(0, 8f, 0);
            pathSpeed = pathSpeed * 2;
            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 10f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("path"))
        {
            rb.isKinematic = _collider.isTrigger = true;
            pathSpeed = 30f;
            var airEffectMain = airEffect.main;
            airEffectMain.simulationSpeed = 4f;
        }
    }
}

