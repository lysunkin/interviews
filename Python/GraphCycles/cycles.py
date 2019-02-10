from collections import defaultdict
  
class Graph(): 
  
    def __init__(self, V): 
        self.V = V 
        self.graph = defaultdict(list) 
  
    def addEdge(self, v, w): 
        self.graph[v].append(w)  
        self.graph[w].append(v)  
  
    def isCyclicUtil(self, v, visited, parent): 
  
        visited[v] = True
  
        for i in self.graph[v]: 
            if visited[i] == False: 
                if self.isCyclicUtil(i, visited, v) == True: 
                    return True
            elif i != parent: 
                return True
  
        return False
  
    # Returns true if the graph is a tree, elsewhere false. 
    def isTree(self): 
        visited = [False] * self.V 
  
        if self.isCyclicUtil(0, visited, -1) == True: 
            return False
  
        for i in range(self.V): 
            if visited[i] == False: 
                return False
  
        return True

g1 = Graph(5) 
g1.addEdge(1, 0) 
g1.addEdge(0, 2) 
g1.addEdge(0, 3) 
g1.addEdge(3, 4) 
print "Graph is a Tree" if g1.isTree() == True else "Graph is a not a Tree"
  
g2 = Graph(5) 
g2.addEdge(1, 0) 
g2.addEdge(0, 2) 
g2.addEdge(2, 1) 
g2.addEdge(0, 3) 
g2.addEdge(3, 4) 
print "Graph is a Tree" if g2.isTree() == True else "Graph is a not a Tree"

g1 = Graph(5) 
g1.addEdge(1, 0) 
g1.addEdge(0, 2) 
g1.addEdge(3, 4) 
print "Graph is a Tree" if g1.isTree() == True else "Graph is a not a Tree"
