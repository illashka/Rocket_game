using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RocketScript : MonoBehaviour
{
    //SF - для доступа к полю rotSpeed из инспектора (не делая её public)
    [SerializeField]float rotSpeed = 100.0f;//скорость вращения ракеты по умолчанию
    [SerializeField]float flySpeed = 1200.0f;//скорость полета ракеты по умолчанию
    Rigidbody rigidBody;
    enum State {Playing, Dead, NextLevel}//три возможных состояния игрока
    State state = State.Playing; //По умолчанию игрок находится в процессе игры

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            Launch();
            Rotation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing)
        {
            return;
        }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("All fine");
            break;

            case "Finish":
                finish();
            break;

            default:
                lose();
            break;
        }
    }

    void finish()
    {
        state = State.NextLevel;
        Invoke("NextLevel", 2f);
    }

    void lose()
    {
        state = State.Dead;
        Invoke("FirstLevel", 2f);
    }

    void NextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 1;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }
    void FirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void Launch()
    {
        if(Input.GetKey(KeyCode.Space)) //зажат пробел
        {

            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);//Тело летит вверх относительно локальных координат
        }
        else
        {

        }
    }
    void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;//Чтобы скорость поворота не зависела от FPS

        rigidBody.freezeRotation = true; //Физическая инерация мешает нормальному вращению
        if(Input.GetKey(KeyCode.A)) //поворот влево
        {
            transform.Rotate(Vector3.left * rotationSpeed);
        }
        
        else if(Input.GetKey(KeyCode.D)) //поворот вправо
        {
            transform.Rotate(Vector3.right * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }
}
