using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJFUK
{
    public class AudienceManager : MonoBehaviour
    {
        public GameObject[] audiences;

        public GameObject[] audiencePoints;

        public GameObject[] spawnPoints;

        public Transform audiencesParent;

        public Transform lookPoint;

        public AudienceState audienceState = AudienceState.Idle;

        List<Audience> audienceList = new List<Audience>();

        int audienceIndex = 0;

        void Start()
        {
            SpawnFirstAudiences();

            //Invoke("SpawnNextAudience", 2.0f);
        }

        void SpawnFirstAudiences()
        {
            while(true)
            {
                SpawnAudience(audiencePoints[audienceIndex].transform.position, audiencePoints[audienceIndex].transform.rotation, true);

                audienceIndex++;

                if(audienceIndex == 9)
                {
                    break;
                }
            }
        }

        Audience SpawnAudience(Vector3 spawnPosition, Quaternion spawnRotation, bool checkDuplicate = false)
        {
            GameObject randomAudience = GetRandomAudience();

            if (checkDuplicate)
            {
                bool check = true;
                while (check)
                {
                    check = false;

                    foreach (Audience a in audienceList)
                    {
                        if(a.gameObject.name.Contains(randomAudience.name))
                        {
                            check = true;
                            randomAudience = GetRandomAudience();
                            break;
                        }
                    }
                }
            }

            GameObject audienceObject = Instantiate(randomAudience, spawnPosition, spawnRotation);
            audienceObject.transform.SetParent(audiencesParent);

            Audience audience = audienceObject.GetComponent<Audience>();
            audience.audienceState = AudienceState.Idle;
            audienceList.Add(audience);

            return audience;
        }

        GameObject GetRandomAudience()
        {
            return audiences[Random.Range(0, audiences.Length)];
        }

        public void SpawnNextAudience()
        {
            if(audienceIndex >= audiencePoints.Length)
            {
                return;
            }

            Vector3 spawnPosition = audiencePoints[audienceIndex].transform.position;

            // Find closest spawn point
            GameObject closestSpawnPoint = null;
            float closestDistance = Mathf.Infinity;
            foreach (GameObject spawnPoint in spawnPoints)
            {
                float distanceToSpawnPoint = Vector3.Distance(spawnPosition, spawnPoint.transform.position);

                if (distanceToSpawnPoint < closestDistance)
                {
                    closestDistance = distanceToSpawnPoint;
                    closestSpawnPoint = spawnPoint;
                }
            }

            Audience audience = SpawnAudience(closestSpawnPoint.transform.position, closestSpawnPoint.transform.rotation);
            audience.WalkTo(spawnPosition);

            audienceIndex++;
        }

        public void SetAudienceState(AudienceState audienceState)
        {
            this.audienceState = audienceState;

            foreach(Audience audience in audienceList)
            {
                if(audience.audienceState != AudienceState.Walk)
                {
                    audience.audienceState = audienceState;

                    audience.SetAnimation();
                }
            }
        }
    }
}
