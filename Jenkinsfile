pipeline {
    agent any
    
     environment{
        scannerHome = tool name: 'sonar_scanner_dotnet', type: 'hudson.plugins.sonar.MsBuildSQRunnerInstallation'
    }
    
    stages {
        stage('checkout'){
            steps{
               checkout scm
            }
        }
        stage('nuget'){
            steps{
              bat script: 'dotnet restore' 
            }
        }
       
        stage('start sonarqube analysis'){
            steps{
                withSonarQubeEnv('Test_Sonar'){
                   echo "aa --  ${scannerhome}"
                    bat script: 'dotnet "%scannerhome%/sonarscanner.msbuild.dll" begin /k:dotnetassignment /n:dotnetassignment /v:1.0'
                }       
            }
        }
        stage('Build'){
            steps{
                echo "building the projet"
                bat label: '', script: 'dotnet build' 
            }
        }
         stage('sonarqube analysis end'){
             steps{
               withSonarQubeEnv('Test_Sonar'){
                     bat script: 'dotnet "%scannerHome%/SonarScanner.MSBuild.dll"  end'
                 }   
             }
         }
        stage('release artifacts'){
            steps{
                bat label: '', script: 'dotnet publish -c Release -o ./WebApplication4/vandnagarg' 
            }
        }
        stage('docker image'){
            steps{
                bat script: 'docker build -t dtr.nagarro.com:443/i-vandnagarg-master:%BUILD_NUMBER% ./WebApplication4'
            }
        }
        stage('Push to dtr'){
            steps{
                bat script:'docker push dtr.nagarro.com:443/i-vandnagarg-master:%BUILD_NUMBER%'
                
            }
        }
        stage('stop running container'){
        steps{
                bat label: '', script: '''FOR /F  %%i IN ('docker ps ^| findstr 6200') do set containerId=%%i
        		echo %containerId%
        		If "%containerId%" == "" (
        		  echo "No Container running"
        		) ELSE (
        		  docker stop %ContainerId%
        		  docker rm -f %ContainerId%
        		)'''
            }
        }
        stage('docker deployment'){
            steps{
                bat script: 'docker run --name c_vandnagarg_master -d -p 6200:80 dtr.nagarro.com:443/i-vandnagarg-master:%BUILD_NUMBER%'
            }
        }
		
         stage('helm charts deployment'){
            steps{
			withEnv(['KUBECONFIG="./config"']){
                bat label: '', script: 'helm install  nagphelm --generate-name --set image.repository=dtr.nagarro.com:443/i-vandnagarg-master:%BUILD_NUMBER% --set nodeport=30157'
                }
            }
        }
    }
}