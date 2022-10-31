using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoissonDiskSampling
{

    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int permittedAttempts = 30)
    {
        float cellSize              = radius/Mathf.Sqrt(2);

        // Grid for finding which cell a point is within
        int[,] grid                 = new int[Mathf.CeilToInt(sampleRegionSize.x/cellSize), Mathf.CeilToInt(sampleRegionSize.y/cellSize)];

        List<Vector2> points        = new List<Vector2>();
        List<Vector2> spawnPoints   = new List<Vector2>();

        spawnPoints.Add(sampleRegionSize/2);

        // Keep attempting to spawn a point around the centre
        while(spawnPoints.Count > 0){

            int spawnIndex          = Random.Range(0, spawnPoints.Count);

            Vector2 spawnCentre     = spawnPoints[spawnIndex];
            bool candidateAccepted  = false;

            // Attempt to find a valid location within the number of permitted attempts
            for(int i = 0; i < permittedAttempts; i++){

                float angle         = Random.value * Mathf.PI * 2;
                Vector2 direction   = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate   = spawnCentre + direction * Random.Range(radius, 2 * radius);


                if(IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)){

                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    // Note which cell the point is in
                    grid[(int)(candidate.x/cellSize), (int)(candidate.y/cellSize)]  = points.Count;
                    candidateAccepted   = true;
                    break;
                }
            }

            if(!candidateAccepted){

                spawnPoints.RemoveAt(spawnIndex);
            }
        }

        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid){

        // If inside the region...
        if(candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y){

            int cellX           = (int)(candidate.x/cellSize);
            int cellY           = (int)(candidate.y/cellSize);

            // Define search area around the proposed point
            int searchStartX    = Mathf.Max(0, cellX - 2);
            int searchEndX      = Mathf.Min(cellX + 2, grid.GetLength(0)-1);
            int searchStartY    = Mathf.Max(0, cellY - 2);
            int searchEndY      = Mathf.Min(cellY + 2, grid.GetLength(1)-1);

            for(int x = searchStartX; x <= searchEndX; x++){  
                for(int y = searchStartY; y <= searchEndY; y++){

                    int pointIndex  = grid[x,y] - 1;

                    if(pointIndex != -1){

                        float distance  = (candidate - points[pointIndex]).magnitude;

                        if(distance < radius){
                            
                            // Candidate is too close to the point
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        return false;
    }
}
