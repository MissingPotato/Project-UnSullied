using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {

	Resolution[] resolutions;

	public Dropdown resolutionDropdown;

	private void Start()
	{
		InitializeResolutions();

	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution rez = resolutions[resolutionIndex];
		Screen.SetResolution(rez.width, rez.height, Screen.fullScreen);
	}

	public void SetQuallity (int quallityIndex)
	{
		QualitySettings.SetQualityLevel(quallityIndex);
	}

	public void SetFullscreen (bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	void InitializeResolutions()
	{
		resolutions = Screen.resolutions;

		int currentResolutionIndex = 0;

		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + "x" + resolutions[i].height;
			options.Add(option);

			if ( resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height )
				currentResolutionIndex = i;
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}

}
