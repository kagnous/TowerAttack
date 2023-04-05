using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseController : EntityController
{
    public override void ApplyDamage(float damage)
    {
        _currentLife -= damage;

        if (_currentLife <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
