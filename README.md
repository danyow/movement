# movement

Unity Tutorial

## [u1-Sliding A Sphere(滑动球体)][u1]

> 注意事项

1. TrailRenderer 是直接引擎自带的
2. 宽度的修改在图表上
3. `transform.localPosition = new Vector3(playerInput.x, 0f, playerInput.y);`里的0f显示不出脱尾，需要设置为0.5f
4. 后面的`transform.localPosition += xxxxx`的0f是可以正确显示的 因为之前为0.5f了 += 0f 还是0.5

## [u2-Physics(物理)][u2]

> 注意事项

1. `｜=`是位或运算

[u1]: https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
[u2]: https://catlikecoding.com/unity/tutorials/movement/physics/
