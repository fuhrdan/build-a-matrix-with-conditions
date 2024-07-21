//*****************************************************************************
//** 2392. Build a Matrix With Conditions                                    **
//**                                                                  - Dan  **
//*****************************************************************************


/**
 * Return an array of arrays of size *returnSize.
 * The sizes of the arrays are returned as *returnColumnSizes array.
 * Note: Both returned array and *columnSizes array must be malloced, assume caller calls free().
 */
int topologicalSort(int n, int** conditions, int conditionsSize, int* conditionsColSize, int* order) {
    int* ind = (int*)calloc(n, sizeof(int));
    int** adjList = (int**)malloc(n * sizeof(int*));
    int* adjListSizes = (int*)calloc(n, sizeof(int));
    
    for (int i = 0; i < n; ++i) {
        adjList[i] = (int*)malloc(n * sizeof(int));
    }
    
    for (int i = 0; i < conditionsSize; ++i) {
        int u = conditions[i][0] - 1;
        int v = conditions[i][1] - 1;
        if(u >= 0 && u < n && v >= 0 && v < n)
        {
            bool exists = false;
            for( int j = 0; j < adjListSizes[u]; ++j)
            {
                if(adjList[u][j] == v)
                {
                    exists = true;
                    break;
                }
            }
            if(!exists)
            {
                adjList[u][adjListSizes[u]++] = v;
                ind[v]++;
            }
        }
    }
    
    int* stack = (int*)malloc(n * sizeof(int));
    int stackSize = 0;
    for (int i = 0; i < n; ++i) {
        if (ind[i] == 0) {
            stack[stackSize++] = i;
        }
    }
    
    int idx = 0;
    while (stackSize > 0) {
        int node = stack[--stackSize];
        order[idx++] = node;
        for (int i = 0; i < adjListSizes[node]; ++i) {
            int neighbor = adjList[node][i];
            if (--ind[neighbor] == 0) {
                stack[stackSize++] = neighbor;
            }
        }
    }
    
    for (int i = 0; i < n; ++i) {
        free(adjList[i]);
    }
    free(adjList);
    free(adjListSizes);
    free(stack);
    free(ind);
    
    return idx == n;
}

int** buildMatrix(int k, int** rowConditions, int rowConditionsSize, int* rowConditionsColSize, int** colConditions, int colConditionsSize, int* colConditionsColSize, int* returnSize, int** returnColumnSizes) {
    int* rowOrder = (int*)malloc(k * sizeof(int));
    int* colOrder = (int*)malloc(k * sizeof(int));
    
    if (!topologicalSort(k, rowConditions, rowConditionsSize, rowConditionsColSize, rowOrder) ||
        !topologicalSort(k, colConditions, colConditionsSize, colConditionsColSize, colOrder)) {
        *returnSize = 0;
        *returnColumnSizes = NULL;
        free(rowOrder);
        free(colOrder);
        return NULL;
    }
    
    int* rowPosition = (int*)malloc(k * sizeof(int));
    int* colPosition = (int*)malloc(k * sizeof(int));
    for (int i = 0; i < k; ++i) {
        rowPosition[rowOrder[i]] = i;
        colPosition[colOrder[i]] = i;
    }
    
    int** result = (int**)malloc(k * sizeof(int*));
    for (int i = 0; i < k; ++i) {
        result[i] = (int*)calloc(k, sizeof(int));
    }
    
    for (int i = 0; i < k; ++i) {
        result[rowPosition[i]][colPosition[i]] = i + 1;
    }
    
    *returnSize = k;
    *returnColumnSizes = (int*)malloc(k * sizeof(int));
    for (int i = 0; i < k; ++i) {
        (*returnColumnSizes)[i] = k;
    }
    
    free(rowOrder);
    free(colOrder);
    free(rowPosition);
    free(colPosition);
    
    return result;
}