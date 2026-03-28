using UnityEngine;

namespace DroneSimulator
{
    // A discrete-time PID controller.
    //
    // The controller combines three terms:
    //   P (Proportional) - reacts to the size of the current error.
    //                      A large error produces a large correction. Fast but can overshoot.
    //   I (Integral)     - reacts to how long the error has been accumulating.
    //                      Eliminates the steady-state offset that pure P control leaves behind.
    //   D (Derivative)   - reacts to how quickly the error is changing.
    //                      Dampens oscillations; acts like a brake as the system approaches the target.
    public class PIDController
    {
        private float _proportionalGain;
        private float _integralGain;
        private float _derivativeGain;

        // Running sum of error × dt, clamped to prevent integral windup.
        private float _accumulatedError;
        private readonly float _accumulatedErrorMax;

        // The error value from the previous Compute call, used to estimate the derivative.
        private float _previousError;

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

        // Computes the PID output for the given error.
        // Call once per FixedUpdate; relies on Time.fixedDeltaTime for integration.
        public float Compute(float error)
        {
            // P term: instantaneous response proportional to how far off we are.
            float proportionalOutput = _proportionalGain * error;

            // I term: grow the accumulated error over time and scale by the integral gain.
            // Clamping prevents "integral windup" where a prolonged error makes the
            // integrator so large that the controller overshoots wildly on correction.
            _accumulatedError += error * Time.fixedDeltaTime;
            _accumulatedError = Mathf.Clamp(_accumulatedError, -_accumulatedErrorMax, _accumulatedErrorMax);

            float integralOutput = _integralGain * _accumulatedError;

            // D term: finite-difference estimate of d(error)/dt.
            // Negative d/dt means the error is shrinking — the derivative term subtracts
            // from the output, slowing the correction before it can overshoot.
            float errorChangeRate = (error - _previousError) / Time.fixedDeltaTime;
            _previousError = error;

            float derivativeOutput = _derivativeGain * errorChangeRate;

            return proportionalOutput + integralOutput + derivativeOutput;
        }

        public void Reset()
        {
            _previousError = 0f;
            _accumulatedError = 0f;
        }
    }
}

