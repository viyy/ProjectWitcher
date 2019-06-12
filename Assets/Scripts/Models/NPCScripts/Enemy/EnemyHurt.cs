using UnityEngine;
using UnityEditor;

namespace EnemySpace
{
    public class EnemyHurt
    {
        MeshRenderer headMesh;

        Color noDamage = new Color(255, 231, 172, 255);
        Color lightDamaged = new Color(1, 0.7764706f, 0.6745283f, 1);
        Color mediumDamaged = new Color(1, 0.4705882f, 0.4823529f, 1);
        Color hardDamaged = new Color(0.9176471f, 0.2745098f, 0.2941176f, 1);
        Color test1 = Color.yellow;
        Color test2 = Color.blue;
        Color test3 = Color.green;

        public EnemyHurt(MeshRenderer headMesh)
        {
            this.headMesh = headMesh;
        }

        public void Hurt(float lifePercent)
        {
            if(lifePercent <= 75 && lifePercent > 50)
            {
                headMesh.material.color = lightDamaged;
            }
            else if (lifePercent <= 50 && lifePercent > 25)
            {
                headMesh.material.color = mediumDamaged;
            }
            else if (lifePercent <= 25 && lifePercent > 0)
            {
                headMesh.material.color = hardDamaged;
            }
        }
    }
}
