using System.Net.NetworkInformation;

namespace Base.Utility.Cryptography;

public static class WorkerIdGenerator
{
    /// <summary>
    /// auto generate workerId, try using mac first, if failed, then randomly generate one
    /// </summary>
    /// <returns>workerId</returns>
    public static int GenerateWorkerId(int maxWorkerId=1023)
    {
        try
        {
            //TODO: match by maxWorkerId
            return GenerateWorkerIdBaseOnMac();
        }
        catch
        {
            return GenerateRandomWorkerId(maxWorkerId);
        }
    }

    /// <summary>
    /// use lowest 10 bit of available MAC as workerId
    /// </summary>
    /// <returns>workerId</returns>
    private static int GenerateWorkerIdBaseOnMac()
    {
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //exclude virtual and Loopback
        var firstUpInterface = nics.OrderByDescending(x => x.Speed).FirstOrDefault(x =>
            !x.Description.Contains("Virtual") && x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
            x.OperationalStatus == OperationalStatus.Up);
        if (firstUpInterface == null) throw new Exception("no available mac found");
        var address = firstUpInterface.GetPhysicalAddress();
        var mac = address.GetAddressBytes();

        return (mac[4] & 0B11) << 8 | mac[5] & 0xFF;
    }

    /// <summary>
    /// randomly generate one as workerId
    /// </summary>
    /// <returns></returns>
    private static int GenerateRandomWorkerId(int maxWorkerId)
    {
        return new Random().Next(maxWorkerId + 1);
    }
}
