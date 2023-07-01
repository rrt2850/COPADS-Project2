// Robert Tetreault (rrt2850@g.rit.edu)

/*******************************************************************************************
* File: PrimeGen.cs
* -----------------------------------------------------------------------------------------
* This file contains a class that generates prime numbers with a specified number of bits.
* It also contains a helper method that generates a random BigInteger with a specified 
* number of bits.
* 
* Note: both this file and Extensions.cs have a method called GenerateRandom. This was
* originally done because they were in the same file using polymorphism, but I decided to
* move the GenerateRandom method that takes a range to Extensions.cs because it's only
* used in the IsProbablyPrime method.
********************************************************************************************/

using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

public class PrimeGenerator{
    //  Initialize a new RandomNumberGenerator
    private RandomNumberGenerator rng = RandomNumberGenerator.Create();
    
    /// <summary>
    /// Generates a prime number with the specified number of bits
    /// </summary>
    /// <param name="bits">The number of bits the prime should be</param>
    /// <returns>A BigInteger prime number with the specified number of bits</returns>
    public BigInteger GeneratePrime(int bits){
        BigInteger value;

        value = GenerateRandom(bits);

        // Ensure value is odd
        if (value % 2 == 0) value++;

        // Keep incrementing value by 2 until it is probably prime
        while (!value.IsProbablyPrime(10))  value += 2;

        return value;
    }

    /// <summary>
    /// Generates a random BigInteger with the specified number of bits
    /// </summary>
    /// <param name="bits">The number of bits the BigInteger should be</param>
    /// <returns>A random odd BigInteger with the specified number of bits</returns>
    public BigInteger GenerateRandom(int bits){
        int bytes = bits / 8;                   //  Calculate the number of bytes needed to hold the bits
        byte[] buffer = new byte[bytes + 1];    //  Initialize a new byte array to hold the random bytes, plus one to ensure positive BigInteger
        rng.GetBytes(buffer);                   //  Fill the byte array with random bytes

        buffer[bytes] = 0; // This ensures that the BigInteger will always be positive

        //  Make sure the byte array is large enough by ORing the last byte with 0x80
        //  to make sure the most significant bit is 1
        buffer[bytes - 1] |= 0x80;   //   makes sure the new BigInteger has at least the specified number of bits

        BigInteger newPrime = new BigInteger(buffer);   //  Make a new BigInteger from the byte array

        //  make the BigInteger odd if it isn't already (all prime numbers are odd except 2)
        if (newPrime.IsEven) newPrime++;
    
        return newPrime;
    }

}
