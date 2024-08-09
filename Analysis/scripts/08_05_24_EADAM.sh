#!/bin/bash

cd ../../Backend/MatchingTest

# Define arrays for different values of n and k
n_values=(5000)
k_values=(7 8)
n_sims=1000
today=$(date +%Y-%m-%d)

# Iterate over each combination of n and k
for n in "${n_values[@]}"; do
  for k in "${k_values[@]}"; do
    # Construct and run the command
    dotnet run -- P_EADAM "$n" "$n" "$k" "$n_sims" "2024_08_07_EADAM/${today}_eadam_${n}_${k}"
  done
done