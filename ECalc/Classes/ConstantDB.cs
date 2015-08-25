using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Classes
{
    internal class Constant
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public double Value { get; private set; }

        public Constant(string desc, string name, double val)
        {
                Description = desc;
                Name = name;
                Value = val;
        }
    }


    internal static class ConstantDB
    {
        public static Constant[] Mathematical { get; private set; }
        public static Constant[] Universal { get; private set; }
        public static Constant[] ElectroMagnetic { get; private set; }
        public static Constant[] Atomic { get; private set; }

        static ConstantDB()
        {
            Mathematical = new Constant[]
            {
                new Constant("The number e", "&e", 2.7182818284590452353602874713526624977572470937000d),
                new Constant("The number log[2](e)", "&Log2E", 1.4426950408889634073599246810018921374266459541530d),
                new Constant("The number log[10](e)", "&Log10E", 0.43429448190325182765112891891660508229439700580366d),
                new Constant("The number log[e](2)", "&Ln2", 0.69314718055994530941723212145817656807550013436026d),
                new Constant("The number log[e](10)", "&Ln10", 2.3025850929940456840179914546843642076011014886288d),
                new Constant("The number log[e](pi)", "&LnPi", 1.1447298858494001741434273513530587116472948129153d),
                new Constant("The number log[e](2*pi)/2", "&Ln2PiOver2", 0.91893853320467274178032973640561763986139747363780d),
                new Constant("The number 1/e", "&InvE", 0.36787944117144232159552377016146086744581113103176d),
                new Constant("The number sqrt(e)", "&SqrtE", 1.6487212707001281468486507878141635716537761007101d),
                new Constant("The number sqrt(2)", "&Sqrt2", 1.4142135623730950488016887242096980785696718753769d),
                new Constant("The number sqrt(1/2) = 1/sqrt(2) = sqrt(2)/2", "&Sqrt1Over2", 0.70710678118654752440084436210484903928483593768845d),
                new Constant("The number sqrt(3)/2", "&HalfSqrt3", 0.86602540378443864676372317075293618347140262690520d),
                new Constant("The number pi", "&Pi", 3.1415926535897932384626433832795028841971693993751d),
                new Constant("The number 2*pi", "&Pi2", 6.2831853071795864769252867665590057683943387987502d),
                new Constant("The number 1/pi", "&OneOverPi", 0.31830988618379067153776752674502872406891929148091d),
                new Constant("The number pi/2", "&PiOver2", 1.5707963267948966192313216916397514420985846996876d),
                new Constant("The number pi/4", "&PiOver4", 0.78539816339744830961566084581987572104929234984378d),
                new Constant("The number sqrt(pi)", "&SqrtPi", 1.7724538509055160272981674833411451827975494561224d),
                new Constant("The number sqrt(2pi)", "&Sqrt2Pi", 2.5066282746310005024157652848110452530069867406099d),
                new Constant("The number sqrt(2*pi*e)", "&Sqrt2PiE", 4.1327313541224929384693918842998526494455219169913d),
                new Constant("The number log(sqrt(2*pi))", "&LogSqrt2Pi", 0.91893853320467274178032973640561763986139747363778),
                new Constant("The number log(sqrt(2*pi*e))", "&LogSqrt2PiE", 1.4189385332046727417803297364056176398613974736378d),
                new Constant("The number log(2 * sqrt(e / pi))", "&LogTwoSqrtEOverPi", 0.6207822376352452223455184457816472122518527279025978),
                new Constant("The number 1/pi", "&InvPi", 0.31830988618379067153776752674502872406891929148091d),
                new Constant("The number 2/pi", "&TwoInvPi", 0.63661977236758134307553505349005744813783858296182d),
                new Constant("The number 1/sqrt(pi)", "&InvSqrtPi", 0.56418958354775628694807945156077258584405062932899d),
                new Constant("The number 1/sqrt(2pi)", "&InvSqrt2Pi", 0.39894228040143267793994605993438186847585863116492d),
                new Constant("The number 2/sqrt(pi)", "&TwoInvSqrtPi", 1.1283791670955125738961589031215451716881012586580d),
                new Constant("The number 2 * sqrt(e / pi)", "&TwoSqrtEOverPi", 1.8603827342052657173362492472666631120594218414085755),
                new Constant("The number (pi)/180 - factor to convert from Degree (deg) to Radians (rad).", "&Degree", 0.017453292519943295769236907684886127134428718885417d),
                new Constant("The number (pi)/200 - factor to convert from NewGrad (grad) to Radians (rad).", "&Grad", 0.015707963267948966192313216916397514420985846996876d),
                new Constant("The number ln(10)/20 - factor to convert from Power Decibel (dB) to Neper (Np). Use this version when the Decibel represent a power gain but the compared values are not powers (e.g. amplitude, current, voltage).", "&PowerDecibel", 0.11512925464970228420089957273421821038005507443144d),
                new Constant("The number ln(10)/10 - factor to convert from Neutral Decibel (dB) to Neper (Np). Use this version when either both or neither of the Decibel and the compared values represent powers.", "&NeutralDecibel", 0.23025850929940456840179914546843642076011014886288d),
                new Constant("The Catalan constant", "&Catalan", 0.9159655941772190150546035149323841107741493742816721342664981196217630197762547694794d),
                new Constant("The Euler-Mascheroni constant", "&EulerMascheroni", 0.5772156649015328606065120900824024310421593359399235988057672348849d),
                new Constant("The number (1+sqrt(5))/2, also known as the golden ratio", "&GoldenRatio", 1.6180339887498948482045868343656381177203091798057628621354486227052604628189024497072d),
                new Constant("The Glaisher constant", "&Glaisher", 1.2824271291006226368753425688697917277676889273250011920637400217404063088588264611297d),
                new Constant("The Khinchin constant", "&Khinchin", 2.6854520010653064453097148354817956938203822939944629530511523455572188595371520028011d),
            };
            Universal = new Constant[]
            {
                new Constant("Speed of Light in Vacuum: c_0 = 2.99792458e8 [m s^-1] (defined, exact; 2007 CODATA)", " & SpeedOfLight", 2.99792458e8),
                new Constant("Magnetic Permeability in Vacuum: mu_0 = 4*Pi * 10^-7 [N A^-2 = kg m A^-2 s^-2] (defined, exact; 2007 CODATA)", "&MagneticPermeability", 1.2566370614359172953850573533118011536788677597500e-6),
                new Constant("Electric Permittivity in Vacuum: epsilon_0 = 1/(mu_0*c_0^2) [F m^-1 = A^2 s^4 kg^-1 m^-3] (defined, exact; 2007 CODATA)", "&ElectricPermittivity", 8.8541878171937079244693661186959426889222899381429e-12),
                new Constant("Characteristic Impedance of Vacuum: Z_0 = mu_0*c_0 [Ohm = m^2 kg s^-3 A^-2] (defined, exact; 2007 CODATA)", "&CharacteristicImpedanceVacuum", 376.73031346177065546819840042031930826862350835242),
                new Constant("Newtonian Constant of Gravitation: G = 6.67429e-11 [m^3 kg^-1 s^-2] (2007 CODATA)", " & GravitationalConstant", 6.67429e-11),
                new Constant("Planck's constant: h = 6.62606896e-34 [J s = m^2 kg s^-1] (2007 CODATA)", "&PlancksConstant", 6.62606896e-34),
                new Constant("Reduced Planck's constant: h_bar = h / (2*Pi) [J s = m^2 kg s^-1] (2007 CODATA)", "&DiracsConstant", 1.054571629e-34),
                new Constant("Planck mass: m_p = (h_bar*c_0/G)^(1/2) [kg] (2007 CODATA)", " & PlancksMass", 2.17644e-8),
                new Constant("Planck temperature: T_p = (h_bar*c_0^5/G)^(1/2)/k [K] (2007 CODATA)", " & PlancksTemperature", 1.416786e32),
                new Constant("Planck length: l_p = h_bar/(m_p*c_0) [m] (2007 CODATA)", " & PlancksLength", 1.616253e-35),
                new Constant("Planck time: t_p = l_p/c_0 [s] (2007 CODATA)", " & PlancksTime", 5.39124e-44),
            };
            ElectroMagnetic = new Constant[]
            {
                new Constant("Elementary Electron Charge: e = 1.602176487e-19 [C = A s] (2007 CODATA)", "&ElementaryCharge", 1.602176487e-19),
                new Constant("Magnetic Flux Quantum: theta_0 = h/(2*e) [Wb = m^2 kg s^-2 A^-1] (2007 CODATA)", "&MagneticFluxQuantum", 2.067833668e-15),
                new Constant("Conductance Quantum: G_0 = 2*e^2/h [S = m^-2 kg^-1 s^3 A^2] (2007 CODATA)", "&ConductanceQuantum", 7.7480917005e-5),
                new Constant("Josephson Constant: K_J = 2*e/h [Hz V^-1] (2007 CODATA)", "&JosephsonConstant", 483597.891e9),
                new Constant("Von Klitzing Constant: R_K = h/e^2 [Ohm = m^2 kg s^-3 A^-2] (2007 CODATA)", "&VonKlitzingConstant", 25812.807557),
                new Constant("Bohr Magneton: mu_B = e*h_bar/2*m_e [J T^-1] (2007 CODATA)", "&BohrMagneton", 927.400915e-26),
                new Constant("Nuclear Magneton: mu_N = e*h_bar/2*m_p [J T^-1] (2007 CODATA)", "&NuclearMagneton", 5.05078324e-27),
            };
            Atomic = new Constant[]
            {
                new Constant("Fine Structure Constant: alpha = e^2/4*Pi*e_0*h_bar*c_0 [1] (2007 CODATA)", "&FineStructureConstant", 7.2973525376e-3),
                new Constant("Rydberg Constant: R_infty = alpha^2*m_e*c_0/2*h [m^-1] (2007 CODATA)", "&RydbergConstant", 10973731.568528),
                new Constant("Bor Radius: a_0 = alpha/4*Pi*R_infty [m] (2007 CODATA)", "&BohrRadius", 0.52917720859e-10),
                new Constant("Hartree Energy: E_h = 2*R_infty*h*c_0 [J] (2007 CODATA)", "&HartreeEnergy", 4.35974394e-18),
                new Constant("Quantum of Circulation: h/2*m_e [m^2 s^-1] (2007 CODATA)", "&QuantumOfCirculation", 3.6369475199e-4),
                new Constant("Fermi Coupling Constant: G_F/(h_bar*c_0)^3 [GeV^-2] (2007 CODATA)", "&FermiCouplingConstant", 1.16637e-5),
                new Constant("Weak Mixin Angle: sin^2(theta_W) [1] (2007 CODATA)", "&WeakMixingAngle", 0.22256),
                new Constant("Electron Mass: [kg] (2007 CODATA)", "&ElectronMass", 9.10938215e-31),
                new Constant("Electron Mass Energy Equivalent: [J] (2007 CODATA)", "&ElectronMassEnergyEquivalent", 8.18710438e-14),
                new Constant("Electron Molar Mass: [kg mol^-1] (2007 CODATA)", "&ElectronMolarMass", 5.4857990943e-7),
                new Constant("Electron Compton Wavelength: [m] (2007 CODATA)", "&ComptonWavelength", 2.4263102175e-12),
                new Constant("Classical Electron Radius: [m] (2007 CODATA)", "&ClassicalElectronRadius", 2.8179402894e-15),
                new Constant("Tomson Cross Section: [m^2] (2002 CODATA)", "&ThomsonCrossSection", 0.6652458558e-28),
                new Constant("Electron Magnetic Moment: [J T^-1] (2007 CODATA)", "&ElectronMagneticMoment", -928.476377e-26),
                new Constant("Electon G-Factor: [1] (2007 CODATA)", "&ElectronGFactor", -2.0023193043622),
                new Constant("Muon Mass: [kg] (2007 CODATA)", "&MuonMass", 1.88353130e-28),
                new Constant("Muon Mass Energy Equivalent: [J] (2007 CODATA)", "&MuonMassEnegryEquivalent", 1.692833511e-11),
                new Constant("Muon Molar Mass: [kg mol^-1] (2007 CODATA)", "&MuonMolarMass", 0.1134289256e-3),
                new Constant("Muon Compton Wavelength: [m] (2007 CODATA)", "&MuonComptonWavelength", 11.73444104e-15),
                new Constant("Muon Magnetic Moment: [J T^-1] (2007 CODATA)", "&MuonMagneticMoment", -4.49044786e-26),
                new Constant("Muon G-Factor: [1] (2007 CODATA)", "&MuonGFactor", -2.0023318414),
                new Constant("Tau Mass: [kg] (2007 CODATA)", "&TauMass", 3.16777e-27),
                new Constant("Tau Mass Energy Equivalent: [J] (2007 CODATA)", "&TauMassEnergyEquivalent", 2.84705e-10),
                new Constant("Tau Molar Mass: [kg mol^-1] (2007 CODATA)", "&TauMolarMass", 1.90768e-3),
                new Constant("Tau Compton Wavelength: [m] (2007 CODATA)", "&TauComptonWavelength", 0.69772e-15),
                new Constant("Proton Mass: [kg] (2007 CODATA)", "&ProtonMass", 1.672621637e-27),
                new Constant("Proton Mass Energy Equivalent: [J] (2007 CODATA)", "&ProtonMassEnergyEquivalent", 1.503277359e-10),
                new Constant("Proton Molar Mass: [kg mol^-1] (2007 CODATA)", "&ProtonMolarMass", 1.00727646677e-3),
                new Constant("Proton Compton Wavelength: [m] (2007 CODATA)", "&ProtonComptonWavelength", 1.3214098446e-15),
                new Constant("Proton Magnetic Moment: [J T^-1] (2007 CODATA)", "&ProtonMagneticMoment", 1.410606662e-26),
                new Constant("Proton G-Factor: [1] (2007 CODATA)", "&ProtonGFactor", 5.585694713),
                new Constant("Proton Shielded Magnetic Moment: [J T^-1] (2007 CODATA)", "&ShieldedProtonMagneticMoment", 1.410570419e-26),
                new Constant("Proton Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "&ProtonGyromagneticRatio", 2.675222099e8),
                new Constant("Proton Shielded Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "&ShieldedProtonGyromagneticRatio", 2.675153362e8),
                new Constant("Neutron Mass: [kg] (2007 CODATA)", "&NeutronMass", 1.674927212e-27),
                new Constant("Neutron Mass Energy Equivalent: [J] (2007 CODATA)", "&NeutronMassEnegryEquivalent", 1.505349506e-10),
                new Constant("Neutron Molar Mass: [kg mol^-1] (2007 CODATA)", "&NeutronMolarMass", 1.00866491597e-3),
                new Constant("Neuron Compton Wavelength: [m] (2007 CODATA)", "&NeutronComptonWavelength", 1.3195908951e-1),
                new Constant("Neutron Magnetic Moment: [J T^-1] (2007 CODATA)", "&NeutronMagneticMoment", -0.96623641e-26),
                new Constant("Neutron G-Factor: [1] (2007 CODATA)", "&NeutronGFactor", -3.82608545),
                new Constant("Neutron Gyro-Magnetic Ratio: [s^-1 T^-1] (2007 CODATA)", "&NeutronGyromagneticRatio", 1.83247185e8),
                new Constant("Deuteron Mass: [kg] (2007 CODATA)", "&DeuteronMass", 3.34358320e-27),
                new Constant("Deuteron Mass Energy Equivalent: [J] (2007 CODATA)", "&DeuteronMassEnegryEquivalent", 3.00506272e-10),
                new Constant("Deuteron Molar Mass: [kg mol^-1] (2007 CODATA)", "&DeuteronMolarMass", 2.013553212725e-3),
                new Constant("Deuteron Magnetic Moment: [J T^-1] (2007 CODATA)", "&DeuteronMagneticMoment", 0.433073465e-26),
                new Constant("Helion Mass: [kg] (2007 CODATA)", "&HelionMass", 5.00641192e-27),
                new Constant("Helion Mass Energy Equivalent: [J] (2007 CODATA)", "&HelionMassEnegryEquivalent", 4.49953864e-10),
                new Constant("Helion Molar Mass: [kg mol^-1] (2007 CODATA)", "&HelionMolarMass", 3.0149322473e-3),
            };
        }

        public static double Lookup(string name)
        {
            var q1 = from i in Mathematical where i.Name == name select i.Value;
            var q2 = from i in Universal where i.Name == name select i.Value;
            var q3 = from i in ElectroMagnetic where i.Name == name select i.Value;
            var q4 = from i in Atomic where i.Name == name select i.Value;

            if (q1.Count() > 0) return q1.First();
            else if (q2.Count() > 0) return q2.First();
            else if (q3.Count() > 0) return q3.First();
            else if (q4.Count() > 0) return q4.First();
            else throw new ArgumentException("Constant not found");
        }
    }
}
