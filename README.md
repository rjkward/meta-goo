# meta-goo

![such blob](./.github/meta-goo.gif)

a nice little compute shader project for a viscous character controller. developed for a katamari damacy inspired game jam game.

the metaball isosurface is generated using marching cubes on the gpu and then rendered without cpu readbacks. as far as the cpu is concerned the goo is just a bunch of sphere colliders with rigidbody physics. 

### TODO
- change physics to DOTS
- sensibly adjust voxel count based on bounds to get tri count under control
- calculate normals from metaball field directly instead of approximating derivatives from voxel corner values (this would also allow the use of metaball functions with finite support to be properly lit)
- surface colour = blended colours per ball based on field strength
