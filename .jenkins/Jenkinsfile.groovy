pipeline {
	agent {
		label 'unity'
	}
	options {
		disableConcurrentBuilds()
		disableResume()
	}
	stages {
		stage('Index workspace') {
			steps {
				script {
					unityProject (
							// define unity project location relative to repository
							LOCATION : '',

							// If given, automatically use these credentials to license a free Unity version.
							UNITY_CREDENTIALS : 'Slothsoft-Unity',
							EMAIL_CREDENTIALS : 'Slothsoft-Google',

							// use auto-versioning based on tags+commits
							AUTOVERSION : 'git',

							// automatically create C# docs
							BUILD_DOCUMENTATION : '0',

							// automatically run Unity Test Runner
							TEST_UNITY : '0',

							// automatically run dotnet format
							TEST_FORMATTING : '0',
							FORMATTING_EXCLUDE : 'Library Assets/Plugins Packages',

							// which executables to create
							BUILD_FOR_WINDOWS : '1',
							BUILD_FOR_LINUX : '1',
							BUILD_FOR_MAC : '1',
							BUILD_FOR_WEBGL : '1',
							BUILD_FOR_ANDROID : '0',

							// which platforms to deploy to
							DEPLOY_TO_STEAM : '0',
							DEPLOY_TO_ITCH : '1',
							DEPLOY_ON_FAILURE : '1',

							// configration for deploying to steam
							STEAM_ID : '',
							STEAM_DEPOT_WINDOWS : '',
							STEAM_DEPOT_LINUX : '',
							STEAM_DEPOT_MAC : '',
							STEAM_CREDENTIALS : '',

							// configuration for deploying to itch
							ITCH_ID : 'gregorsoenn/press-for-peace',
							ITCH_CREDENTIALS : 'Faulo-itch.io',

							// configuration for deploying to Discord
							REPORT_TO_DISCORD : '0',
							DISCORD_WEBHOOK : '',
							)
				}
			}
		}
	}
}