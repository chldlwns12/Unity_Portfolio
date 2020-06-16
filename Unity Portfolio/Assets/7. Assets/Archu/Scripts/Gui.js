#pragma strict

var weapon : Transform;
var halo : Transform;
var wings : Transform;
var wingsMesh : Transform;

function Start () {
	animation.CrossFade("Idle"); 
	wings.animation.CrossFade("Idle");  
}

function Update () {

}

function OnGUI () {
	//顯示武器
	if(GUILayout.Button("Show/Hide Weapon")){
		if(weapon.renderer.enabled == true)
			weapon.renderer.enabled = false;
		else
			weapon.renderer.enabled = true;
    }
	//顯示光環
	if(GUI.Button(Rect(140, 0, 120, 20), "Show/Hide Halo")){
		if(halo.renderer.enabled == true)
			halo.renderer.enabled = false;
		else
			halo.renderer.enabled = true;
    }
	//顯示翅膀
	if(GUI.Button(Rect(280, 0, 120, 20), "Show/Hide wings")){
		if(wingsMesh.renderer.enabled == true)
			wingsMesh.renderer.enabled = false;
		else
			wingsMesh.renderer.enabled = true; 
    }
	//動作
	if(GUILayout.Button("Idle")){
        animation.CrossFade("Idle"); 
        wings.animation.CrossFade("Idle");
    }
	if(GUILayout.Button("Idle2")){
        animation.CrossFade("Idle2");
        wings.animation.CrossFade("Idle2");
    }
	if(GUILayout.Button("Walk")){
		weapon.renderer.enabled = true;
        animation.CrossFade("Walk"); 
        wings.animation.CrossFade("Walk");
    }
	if(GUILayout.Button("Walk2")){
		weapon.renderer.enabled = false;
        animation.CrossFade("Walk2"); 
        wings.animation.CrossFade("Walk2");
    }
	if(GUILayout.Button("Run")){
		weapon.renderer.enabled = true;
        animation.CrossFade("Run"); 
        wings.animation.CrossFade("Run");
    }
	if(GUILayout.Button("Run2")){
		weapon.renderer.enabled = false;
        animation.CrossFade("Run2"); 
        wings.animation.CrossFade("Run2");
    }
	if(GUILayout.Button("Jump")){
		weapon.renderer.enabled = true;
        animation.CrossFade("Jump"); 
        wings.animation.CrossFade("Jump");
    }
	if(GUILayout.Button("Jump2")){
		weapon.renderer.enabled = false;
        animation.CrossFade("Jump2"); 
        wings.animation.CrossFade("Jump2");
    }
	if(GUILayout.Button("Attack1")){
        animation.CrossFade("Attack1");
        wings.animation.CrossFade("Attack1");
    }
	if(GUILayout.Button("Attack2")){
        animation.CrossFade("Attack2"); 
        wings.animation.CrossFade("Attack2");
    }
	if(GUILayout.Button("Attack3-1")){
        animation.CrossFade("Attack3-1"); 
        wings.animation.CrossFade("Attack3-1");
    }
	if(GUILayout.Button("Attack3-2")){
        animation.CrossFade("Attack3-2"); 
        wings.animation.CrossFade("Attack3-2");
    }
	if(GUILayout.Button("Attack3-3")){
        animation.CrossFade("Attack3-3"); 
        wings.animation.CrossFade("Attack3-3");
    }
	if(GUILayout.Button("Attack4")){
        animation.CrossFade("Attack4"); 
        wings.animation.CrossFade("Attack4");
    }
	if(GUILayout.Button("Wound")){
        animation.CrossFade("Wound"); 
        wings.animation.CrossFade("Wound");
    }
	if(GUILayout.Button("Stun")){
        animation.CrossFade("Stun"); 
        wings.animation.CrossFade("Stun");
    }
	if(GUILayout.Button("HitAway")){
        animation.CrossFade("HitAway"); 
        wings.animation.CrossFade("HitAway");
    }
	if(GUILayout.Button("HitAwayUp")){
        animation.CrossFade("HitAwayUp"); 
        wings.animation.CrossFade("HitAwayUp");
    }
	if(GUILayout.Button("Death")){
        animation.CrossFade("Death"); 
        wings.animation.CrossFade("Death");
    }
	if(GUILayout.Button("Magic")){
        animation.CrossFade("Magic"); 
        wings.animation.CrossFade("Magic");
    }
	if(GUILayout.Button("Fire")){
        animation.CrossFade("Fire"); 
        wings.animation.CrossFade("Fire");
    }
}
