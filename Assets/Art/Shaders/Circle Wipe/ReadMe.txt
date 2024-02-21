In the UniversalRenderData of the current render pipeline, add fullscreen render pass and attach the circlwipematerial to it
"_Radius": Determines how big the visible part of the screen is. Is relative to aspect ratio, but I haven't bothered to account for it in the code.
"_AspectRatio": Used to account for different monitor aspect ratios. The circle wipe will not be round if the aspect ratio is not correct.
"_Speed": I forgot what this does. Don't change it.
"_Offset": Used to offset the visible part of the screen. Useful if you want to focus on a character during the transition. Also affects the _Radius, so use carefully.