#!/bin/bash

cd ../../Backend/MatchingTest

# Define arrays for different values of n and k
n_values=(6000)
k_values=(8 9 10 11 12 13 14 15)
n_sims=1000
today=$(date +%Y-%m-%d)

# Iterate over each combination of n and k
for n in "${n_values[@]}"; do
  for k in "${k_values[@]}"; do
    # Construct and run the command
    dotnet run -- P_EADAM "$n" "$n" "$k" "$n_sims" "${today}_eadam_${n}_${k}"
  done
done