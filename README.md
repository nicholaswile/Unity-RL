# Unity-RL
A prototype demonstrating reinforcement learning for agent-based artificial intelligence with Unity ML-Agents. 

## Requirements
* Unity 2021.3 
* Python 3.8.13
* PyTorch 1.7.1 (with CUDA 11.0)
* ML-Agents 0.30.0

To run the project, first clone this repository, then: 

### Download Unity
Use Unity 2021.3. You can download it from Unity Hub, which can be installed from the <a href="https://unity.com/releases/editor/archive">Unity website</a>.

### Download Python and create a virtual environment
I struggled to get this project to work with any version of Python other than Python 3.8.13 (because I am using an older version of Unity). Originally you could download the Windows installer from the <a href="https://www.python.org/downloads/release/python-3813/">Python website</a>. If you have a 64-bit machine, it is highly recommended you select the x86-64 installer. However, Python does not provide binary installers for this version anymore; you can download the installer from <a href="https://github.com/adang1345/PythonWindows/tree/master?tab=readme-ov-file">this GitHub repository</a>. 

Then create a virtual environment using Python 3.8.13 inside of the root of the Unity project directory (the Unity RL folder you just cloned):
```bash
$ [YOUR_PATH]\\Python\\Python38\\python.exe -m venv [YOUR_VENV_NAME]
```

Then activate the venv:
```bash
$ [YOUR_VENV_NAME]\\Scripts\\activate
$ source [YOUR_VENV_NAME]\\Scripts\\activate
```

Then confirm that the venv has been created for the correct Python version:
```bash
$ python --version
Python 3.8.13
```

And upgrade pip using Python 3.8.13:
```bash
$ python -m pip install --upgrade pip
```

### Install PyTorch
```bash
$ pip3 install torch~=1.7.1 -f https://download.pytorch.org/whl/torch_stable.html
```

### Install ML-Agents
```bash
$ python -m pip install mlagents==0.30.0
```

### Troubleshooting ML-Agents dependency issues
Verify that ML-Agents has been installed correctly using the command:
```bash
$ mlagents-learn --help
```

A list of command line arguments should display. If you get errors, then here is how I fixed it:

I had to downgrade protobuf,
```bash
$ pip install protobuf==3.20
```

And install the six package:
```bash
$ python -m pip install six 
```

Doing this fixed the dependency and version errors for ML-Agents.

### Installing CUDA
You probably won't need to do this, I didn't, because I already have CUDA and even if I didn't I don't think a separate download is required anymore for ML-Agents 0.30.0. But just in case, if running the mlagents-learn --help command generated warnings about missing CUDA libraries, you may ignore this if you only want to use the CPU for processing. 

If you get these warnings and do have an NVIDIA GPU and want to use CUDA, you can download the CUDA Toolkit from the <a href="https://developer.nvidia.com/cuda-11.0-download-archive">NVIDIA download page</a>. If you still get warnings about cuDNN, you may install it from the <a href="https://developer.nvidia.com/cudnn">cuDNN page</a> and copy the include, lib, and bin folders into the CUDA v11.0 folder. 

## Contributors
### Nicholas Wile
Kennesaw State University Department of Computer Science.

## Citations

### Unity ML-Agents Package
Unity Technologies. (2023). *Unity Machine Learning Agents*. Retrieved from https://github.com/Unity-Technologies/ml-agents

### Research Paper
Juliani, A., Berges, V., Vckay, E., Gao, Y., Henry, H., Mattar, M., & Lange, D. (2018). *Unity: A General Platform for Intelligent Agents*. arXiv preprint arXiv:1809.02627.