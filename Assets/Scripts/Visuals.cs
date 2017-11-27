using System.Collections;
using System.Collections.Generic;
using UnityEngine.PostProcessing;
using UnityEngine;
using Pathfinding;
using View;

public class Visuals : MonoBehaviour {
  public static Visuals current;

  [Range(1, 24)]
  public float visibility = 5;
  public bool hasClouds = true;
  public Color background;
  public PostProcessingProfile profile;
  public float transitionSpeed = 2f;

  public float visibilityUnits { get { return (grid.square) * visibility; } }

  private Pathfinding.Grid grid;

  private PostProcessingProfile profileOriginal;
  private PostProcessingBehaviour normalPost;
  private PostProcessingProfile profileSecondary;
  private float activeContribution = 1;
  private float activeContributionVel;

  void Awake() {
    grid = GetComponent<Pathfinding.Grid>();
    current = this;
  }

  void Start() {
    normalPost = Controller.current.viewport.viewport.GetComponent<PostProcessingBehaviour>();
    profileOriginal = profile;
    normalPost.profile = Instantiate(profileOriginal);
  }

  void Update() {
    activeContribution = Mathf.SmoothDamp(activeContribution, profileSecondary != null ? 0 : profileOriginal.userLut.settings.contribution, ref activeContributionVel, transitionSpeed);

    if (profileSecondary != null && activeContribution < 0.01f) {
      profileOriginal = profileSecondary;
      profile = Instantiate(profileOriginal);
      profileSecondary = null;
      normalPost.profile = profile;
    }

    UserLutModel.Settings lut = normalPost.profile.userLut.settings;
    lut.contribution = activeContribution;
    normalPost.profile.userLut.settings = lut;
  }

  public void Switch(PostProcessingProfile profile, int visibility = 0) {
    profileSecondary = profile;
    if (visibility > 0) this.visibility = visibility;
  }

  public bool Check(PostProcessingProfile profile) {
    if (profileSecondary != null && profileSecondary == profile) return false;
    if (profileOriginal == profile && profileSecondary != null) return true;
    if (profileOriginal == profile) return false;
    return true;
  }
}
