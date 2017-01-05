# 3d-rendering
A small C# library for manipulating objects in 3d space and rendering them

This is partially included in the header comments of Object3d.cs and Primitaves.cs.

To use this library, and begin rendering objects in 3d space, these are the steps that should be followed:

1) Create an object3d instance (I have included three primitave shapes: Cube, Tetrahedron, and GridFloor) at some point in space, say (0,0,0). It will have a scale of 25 units and will have no initial rotation.

2) Create a Camera object at some other point which faces the cube. This particular camera will be placed at (0, 50, -200) and will have no rotation. We'll set the render size to be 1300x400 and have a background color of LightSlateGray (From System.Drawing.Color)

3) Next we need to tell the camera that it should render the cube or else it won't know it's there. We do this by adding it to the camera's renderQueue.

4) The last step is to tell the camera to actually render the scene that it sees and assign that information to an Image object. By default the camera will render local axis gizmos and will be in perspective mode. This can be changed by changing the "renderGizmos" and "orthographic" properties of the camera.

example code:

```C#
Cube myCube = new Cube(new Vector3(0, 0, 0),  new Vector3(0, 0, 0), new Vector3(25, 25, 25)); //step 1
mainCamera = new Camera(300, new Vector3(0, 50, -200), new Vector3(0, 0, 0), Color.LightSlateGray, 1300, 400);  //step 2
mainCamera.renderQueue.Add(myCube); //step 3
System.Drawing.Image myFirstScene = mainCamera.Render(); //step 4
```

This project borrows a lot of names from the [UnityEngine](https://unity3d.com/) library however it is not otherwise related. This is my original code and was created as a learning experience for myself and shared for others.
