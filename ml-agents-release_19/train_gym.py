from gym_unity.envs import UnityToGymWrapper
from mlagents_envs.environment import UnityEnvironment
from mlagets_envs.side_channel.engine_configuration_channel import \
    EngineConfigurationChannel
from stable_baselines3 import PPO

channel = EngineConfigurationChannel()
unity_env = UnityEnvironment("../Unity_Build/SelfAssembly", side_channels=[channel])
channel.set_configuration_parameters(time_scale=20.0)

env = UnityToGymWrapper(unity_env)

model = PPO("CNNPolicy", env, verbose=1)

model.learn(total_timesteps=30000)

state = env.reset()
while True:
    env.render()
    action, _ = model.predict(state)

    state, reward, done, info = env.step(action)
    print("reward:", reward)

    if done:
        print("done")
        state = env.reset()
