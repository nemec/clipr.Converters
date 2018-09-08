# clipr.Converters

An assortment of TypeConverters for built-in types that are not provided by
default in the .NET Framework. Each of these converters supports conversion
from a string to the appropriate type.

## Usage

Typically, code that supports TypeConverters uses the method
`TypeDescriptor.GetConverter` which finds a `TypeConverter` that
is attached to a class definition via the `TypeConverterAttribute`.
Since these converters apply to built-in .NET types which do not
have converters by default, `TypeDescriptor.GetConverter` will not
be able to find any converter to the type. This means you will need to
either create the converter manually, when you need it, or apply the
`TypeConverterAttribute` to a field or property and use the methods
on the included `ConverterFinder` class to find it.

Manually creating a converter:

```csharp
var converter = new IPAddressConverter();
IPAddress addr = (IPAddress)converter.ConvertFromString("192.168.1.1");
```

Attaching a converter to a property:

```csharp
class Options
{
	[TypeConverter(typeof(IPAddressConverter))]
	public IPAddress DestinationIP { get; set; }
}

class Program
{
	public static void Main(string[] args)
	{
		var property = typeof(Options).GetProperty("DestinationIP");
		var converter = ConverterFinder.GetConverterForProperty(property);
		IPAddress addr = (IPAddress)converter.ConvertFromString("1fbf:0:a88:85a3::ac1f");
	}
}
```

## Converter Finder

```csharp
static TypeConverter ConverterFinder.GetConverterForProperty(PropertyInfo prop)
```

This method attempts to find an appropriate `TypeConverter` for an
un-bound `PropertyInfo`. First, it looks for a `TypeConverterAttribue`
on the property itself. If none exists, it looks up the default `TypeConverter`
for the property type.

```csharp
static TypeConverter ConverterFinder.GetConverterForProperty(PropertyInfo prop, object declaringInstance)
```

This method attempts to find an appropriate `TypeConverter` for an
un-bound `PropertyInfo`. The second parameter must be an instance of
the class where the `PropertyInfo` is bound. First, it looks for a
`TypeConverterAttribue` on the property itself. If the property has
no converter, it looks for one on the object instance. If neither exists,
it looks up the default `TypeConverter` for the property type.

```csharp
static TypeConverter ConverterFinder.GetConverterForField(FieldInfo field)
```

This method attempts to find an appropriate `TypeConverter` for an
un-bound `FieldInfo`. First, it looks for a `TypeConverterAttribue`
on the field itself. If none exists, it looks up the default `TypeConverter`
for the field type.

```csharp
static TypeConverter ConverterFinder.GetConverterForField(FieldInfo field, object declaringInstance)
```

This method attempts to find an appropriate `TypeConverter` for an
un-bound `FieldInfo`. The second parameter must be an instance of
the class where the `FieldInfo` is bound. First, it looks for a
`TypeConverterAttribue` on the field itself. If the field has
no converter, it looks for one on the object instance. If neither exists,
it looks up the default `TypeConverter` for the field type.


## Converters

### IPAddressConverter

This class uses the `System.Net.IPAddress.TryParse` method to turn a string
into a `System.Net.IPAddress` instance. Both IPv4 and IPv6 addresses are supported.

### IPEndPointConverter

This class is similar to the `IPAddressConverter` class, except it also
supports a port. Inputs are converted to the `System.Net.IPEndPoint` class.
For IPv4 addresses, the format is an IP and port separated
by a colon (e.g. `192.168.1.1:8000`). Since IPv6 addresses contains colons
themselves, you must surround the IPv6 address in brackets and append a colon
plus the port at the end (e.g. `[1fbf:0:a88:85a3::ac1f]:8000`).

Unfortunately, the `TypeConverterAttribute` infrastructure does not support
`TypeConverter`s with non-default constructors. Thus, there is no way to
provide a *default* port for the IPEndPoint when one is not supplied in the
input. If your application requires a default port, create a derived class
of the `IPEndPointConverter` and in your default constructor, call the base
constructor overload that takes an integer port value:

```csharp
class IPEndPointConverterWithDefaultPort80 : IPEndPointConverter
{
    public IPEndPointConverterWithDefaultPort80() : base(80) { }
}

class Program
{
	public static void Main(string[] args)
	{
		var converter = new IPEndPointConverterWithDefaultPort80();
        var addr = (IPEndPoint)converter.ConvertFromString("192.168.1.1:8000");
        Console.WriteLine("Explcit port: {0}", addr.Port);
        addr = (IPEndPoint)converter.ConvertFromString("192.168.1.1");
        Console.WriteLine("Default port: {0}", addr.Port);
	}
}
```

### IPHostConverter

The `IPHostConverter` is identical to the `IPEndPointConverter` class except it
also supports looking up a domain name in DNS instead of an IP Address. Inputs
are converted to the `System.Net.IPEndPoint` class. The formats
include `192.168.1.1:8000` and `example.com:455`. Like the `IPEndPointConverter`,
create a derived class if you wish to set a default port. When a domain name is
given as the input and that domain resolves to multiple IP addresses, this converter
only returns the first address in the list, which is ordered by the OS.

### IPHostConverterPreferIPv4

This `IPHostConverter` derived class is identical to its base class except when a
domain is provided as input, it first sorts the resulting IP addresses to put
IPv4 addresses first. If there is no IPv4 address for the domain, the converter
will continue to return an IPv6 address. Inputs are converted to the
`System.Net.IPEndPoint` class. 

### IPHostConverterPreferIPv6

This `IPHostConverter` derived class is identical to its base class except when a
domain is provided as input, it first sorts the resulting IP addresses to put
IPv6 addresses first. If there is no IPv6 address for the domain, the converter
will continue to return an IPv4 address. Inputs are converted to the
`System.Net.IPEndPoint` class. 

### RegexConverter

This `RegexConverter` turns a string into a regular expression object, with default settings.
If a different set of `RegexOptions` must be used, create a derived class of the `RegexConverter`
and in the derived class' default constructor, call the base contstructor with a single `RegexOptions`
parameter. Inputs are converted to the `System.Text.RegularExpressions.Regex` class.

```csharp
class RegexConverterWithCaseInsensitivity : RegexConverter
{
    public RegexConverterWithCaseInsensitivity()
        : base(RegexOptions.IgnoreCase)
    {
    }
}

class Program
{
	public static void Main(string[] args)
	{
        var converter = new RegexConverter();
        var rx = (Regex)converter.ConvertFromInvariantString("th.s");
        var match = rx.Match("send this home");
        Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
        match = rx.Match("send thus home");
        Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
        match = rx.Match("SEND THIS HOME");
        Console.WriteLine("Matched regex case sensitive: {0}", match.Value);
	}
}
```

### TimeSpanConverter

The `TimeSpanConverter` turns a string into an instance of the `System.TimeSpan` class.
It supports any conversion format allowed by `TimeSpan.TryParse` as well as the
following custom formats:

* A plain integer as input will be interpreted as seconds: `45`
* An integer suffixed by 'ms' will be interpreted as milliseconds: `45ms`
* An integer suffixed by 's' will be interpreted as seconds: `45s`
* An integer suffixed by 'm' will be interpreted as minutes: `45m`
* An integer suffixed by 'h' will be interpreted as hours: `45h`
* An integer suffixed by 'd' will be interpreted as days: `45d`

Negative time spans and integer formats are supported (e.g. `-45s`).