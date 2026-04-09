using UnityEngine;

namespace DroneSimulator.Domain
{
    public class PIDController
    {
        private float _proportionalGain;
        private float _integralGain;
        private float _derivativeGain;
        private float _accumulatedError;
        private readonly float _accumulatedErrorMax;
        private float _previousError;
        private bool _isFirstCompute = true;

        public PIDController(
            float proportionalGain,
            float integralGain,
            float derivativeGain,
            float accumulatedErrorMax = 20f)
        {
            _proportionalGain = proportionalGain;
            _integralGain = integralGain;
            _derivativeGain = derivativeGain;
            _accumulatedErrorMax = accumulatedErrorMax;
        }

        public void UpdateGains(float proportionalGain, float integralGain, float derivativeGain)
        {
            _proportionalGain = proportionalGain;
            _integralGain = integralGain;
            _derivativeGain = derivativeGain;
        }

        public float Compute(float error)
        {
            float proportionalOutput = _proportionalGain * error;

            _accumulatedError += error * Time.fixedDeltaTime;
            _accumulatedError = Mathf.Clamp(_accumulatedError, -_accumulatedErrorMax, _accumulatedErrorMax);

            float integralOutput = _integralGain * _accumulatedError;

            float derivativeOutput = 0f;

            if (!_isFirstCompute)
            {
                float errorChangeRate = (error - _previousError) / Time.fixedDeltaTime;
                derivativeOutput = _derivativeGain * errorChangeRate;
            }

            _isFirstCompute = false;
            _previousError = error;

            return proportionalOutput + integralOutput + derivativeOutput;
        }

        public void Reset()
        {
            _isFirstCompute = true;
            _previousError = 0f;
            _accumulatedError = 0f;
        }
    }
}
