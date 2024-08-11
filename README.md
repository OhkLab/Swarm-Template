# Swarm-Template
Template for DRL training with Unity ML-Agents

## How to open unity project
1. Please clone this repository.
```shell
git clone https://github.com/OhkLab/SwarmRobotics_RoundTrip.git
```
2. Open `Unity Hub` > `Add` > Select `/SwarmTemplate` .
3. If you want to see the experimental scene, move to `Assets/SwarmTemplate/Scenes/MA` after opening this unity project.

## Experiment

### Version
* python: 3.8.12

## Requirements
```shell
# Create virtual environment of python
cd ml-agents-release_19
python -m venv venv
source venv/bin/activate

# Install packages.
pip install -e .
pip install -r requirements.txt
```

## Training
```
mlagents-learn ./config/sample/SwarmTemplate.yaml --run-id=Test --force
```