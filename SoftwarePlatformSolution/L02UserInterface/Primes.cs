using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace L02UserInterface;

public class Primes : IEnumerable<long>
{
	private readonly CancellationToken _cancellationToken;

	public Primes(long Max = long.MaxValue, CancellationToken cancellationToken = default)
	{
		this.Max = Max;
		_cancellationToken = cancellationToken;
	}

	public long Max { get; private set; }

	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<long>)this).GetEnumerator();

	public IEnumerator<long> GetEnumerator()
	{
		yield return 1;
		bool bFlag;
		long start = 2;
		while (start < Max)
		{
			bFlag = false;
			var number = start;
			for (int i = 2; i < number; i++)
			{
				if (number % i == 0)
				{
					bFlag = true;
					break;
				}

				_cancellationToken.ThrowIfCancellationRequested();
			}

			if (!bFlag)
			{
				yield return number;
			}
			start++;
		}
	}

}
