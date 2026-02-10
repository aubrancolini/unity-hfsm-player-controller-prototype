# unity-hfsm-player-controller-prototype

Prototype implementation of a 2.5D player controller in Unity using a Hierarchical Finite State Machine (HFSM).
This project was developed as an exploration of HFSM-based gameplay architecture, with a focus on player movement systems and jump mechanics.


---


## Overview

The player controller is implemented using a hierarchical state machine to model high-level movement modes (grounded / airborne) and their related sub-states (idle, walk, jump, fall, air jump).

A shared "PlayerContext" acts as a single source of truth for runtime data and configuration, allowing states to remain focused on behavior while minimizing coupling.

The project uses Unity's "CharacterController", the New Input System, and ScriptableObjects for configuration.


---


## Software Engineering Perspective

Although developed in a game context, this project focuses on general software engineering principles:

- Explicit state modeling instead of conditional logic
- Clear separation between state behavior and shared runtime data
- Centralized context as a single source of truth
- Low coupling between states via transitions driven by shared runtime data rather than direct state-to-state events
- Configuration decoupled from logic using ScriptableObjects


---


## Implemented Systems

- Hierarchical Finite State Machine (HFSM)
  - Root → Grounded / Airborne
  - Nested states for movement and jump phases

- Grounded and airborne movement separation
- Jump buffering
- Coyote time
- Variable jump height (jump hold \& jump cut)
- Air jumps with configurable limits
- Multiple gravity phases (ascending, falling)
- Horizontal control interpolation while airborne
- Input handling decoupled from player logic
- Configuration via ScriptableObjects


---


## Architecture Notes

- \*\*HFSM\*\* is used to express gameplay states explicitly rather than relying on conditional logic.
- \*\*PlayerContext\*\* centralizes runtime state and configuration shared across all states.
- States are responsible only for:
  - state-specific transitions
  - movement and physics behavior relevant to that state

- Unity-specific concerns (input, grounding, movement application) are handled outside the state machine.


---


## Scope

This repository is a \*\*learning and architecture prototype\*\*, not a production-ready character controller.
A future project will revisit the same problem space with:

- a different HFSM framework
- improved separation of responsibilities
- refined state composition and reuse


---


## Tech Stack

- Unity
- C#
- Hierarchical State Machine (HSM framework)


---


## Third-Party Libraries

This project uses the \*\*Unity Hierarchical State Machine\*\* framework by Adam Myhre:
https://github.com/adammyhre/Unity-Hierarchical-StateMachine
The framework was used as-is to explore HFSM-based gameplay architecture and state composition.

