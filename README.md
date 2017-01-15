# CRUST-Physics-Engine

<br/><strong>BIG Disclaimer</strong>
<br/>The graphic assets included here are property of Zeptolab, 2013. I only used them for illustration purposes. They were kind enough to give me the permission to do so and I thank them for that.


<br/><strong>BIGGER Disclaimer</strong>
<br/>Looking at the code now, it seems like a one year-old boy has written this. This is a pretty old code (back to end of 2012.) So my apologies for such badly written code (though, it does it jobs correctly.) It wasn't intended for public release and was only for research. I promise my other projects push to github would be clean and [not] mean.


<br/><strong>Brief</strong>
<br/>CRUST is the heart physics engine behind the Ropossum authoring tool for generating content for physics-based games (Cut the Rope).

<br/>![alt tag](http://www.mohammadshaker.com/assets/img/projects/ropossum/8.jpg)


<br/><strong>Reading and Where to Start</strong>
<br/>CRUST is built after reading <i>Game Physics Engine Development</i> book for Ian Millington. Many ideas (even code) is from Millington's book. Others such as ropes (springs) and rods are from different sources.
<br/>To know more about what CRUST can and can't do look <a href="http://www.mohammadshaker.com/crust.html">here</a>. You can also download the documentation <a href="http://www.slideshare.net/ZGTRZGTR/vr-all">here</a> (in Arabic only, otherwise read the book.) 


<br/><strong>2D and 3D Engine</strong>
<br/>CRUST has the capability to handle both: <strong>2D and 3D physics simulations</strong>. 


<br/><strong>Does this contain Cut the Rope?</strong>
<br/>YES! Go to <a href="https://www.youtube.com/channel/UCvJUfadMoEaZNWdagdMyCRA">Youtube Channel</a> and watch this engine running Cut the Rope.

<br/><strong>Is this what you call Ropossum?</strong>
<br/>Yes and No. In short, NO.
<br/>OK, first I should tell you what's Ropossum (read in full <a href="http://www.mohammadshaker.com/ropossum.html">here</a>). In breif, Ropossum is a full fledge authoring tool for generating content for physics-based games. Ropossum is not a physics engine as you can see. It's an authoring tool for generating content in first place. It can interact with the user or designer. You can ask it for full generation of content or partial generation of content. You can ask it for completing levels with constraints attached. As for latest version (mid 2015), Ropossum has 4 agents for generating content: Pseudo-Random, Simulation-based, Projection-based and Progressive-based. You can read more about them in my <a href="http://www.mohammadshaker.com/publications.html">publications</a>.
<br/>So, as you can see, Ropossum is not a physics engine. Some of those agents are coded in Java since they require some AI and Genetic Algorithms libraries. Those are not included here. Ropossum's physics engine is here: CRUST.


<br/><strong>What version is this?</strong>
<br/>This is my late 2012-early 2013 version of Ropossum. It's built with C# and XNA (Unity was paid at that time :|). I've worked on this version for my graduation thesis. The latest version is mid 2015 (not published yet.)


<br/><strong>Hierarchy and PAAS</strong>
<br/>It's fun to call it. But I have build this engine with <strong>Physics-As-A-Service</strong> mentality. For instance, you can apply different services, effectors, forces, torques on the objects <i>independently</i>. The services don't know about each other and you can apply whatever you want, whatever the compination. You can always remove or add any service realtime. For instance, you can apply ropes to some rigid bodies and in a specific timestamp just remove the rope (spring) service and you have a free falling un-attached bodies. 


<br/><strong>Nice Perks</strong>
<br/>In this version of code, though it's not a Ropossum but a physics engine, you can still play with Cut the Rope and the Authoring tool. You can design your own levels with your own preferences. You can also save your level to your HDD using very simple format. You can also load them for sure also. The system has also the ability to save state in a specific timestamp (something like replayability) and get back to after some minutes. I forgot what else there's. But they are many! Just play and learn!
