# meta-goo
a nice little compute shader project for a viscous character controller. developed for a katamari damacy inspired game jam game. the 'blob' is just a bunch of sphere colliders with rigidbody physics. the metaball isosurface is generated using marching cubes on the gpu and then rendered without cpu readbacks.

### TODO
- change physics to DOTS
- sensibly adjust voxel count based on bounds to get tri count under control
- calculate normals from metaball field directly instead of approximating derivatives from voxel corner values
- surface colour = blended colours per ball based on field strength
