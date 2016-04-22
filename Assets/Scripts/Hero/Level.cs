using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Level : MonoBehaviour
{
        public int cost; //promjenljiva u kojoj se cuva cijena heroja ili upgradea
        public int costSell; //prodaja towera
        public GameObject model;
        public Projectile projectile;
        public float fireRate;
        public float range;
        public float wailingRate;
        public float slowDownFactor;
        public float slowDownDuration;

    public Level(int c, int cs, float fr, float r,float w)
        {
            cost = c;
            costSell = cs;
            fireRate = fr;
            range = r;
            wailingRate = w;
        }

        public int GetCost()
        {
            return cost;
        }

        public int GetCostSell()
        {
            return costSell;
        }

}