# SundayTestWebGallery
This implementation pursued an aditional objective - to <b>reuse image-containing gameobjects</b> while scrolling down the image list.
In other words, there are always as much image objects at the scene, as given screen can possibly show at the same time. 
Images which get out of screen during scrolling are then repositioned to the direction, where new ones are supposed to appear.  
When combined with not only dynamic loading, but also unloading of remote images this could singificantly increase memory performance (not doind unloading at the moment though).

Maybe this was even one of the tasks original intensions, but it still was quite inconvenient to implement in unity.  
All the other features are pretty minimalistic and close to the original description.
