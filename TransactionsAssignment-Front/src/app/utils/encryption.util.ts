import * as CryptoJS from 'crypto-js';

export class EncryptionUtil {
  static encrypt(data: object, key: string): string {
    const keyBytes = CryptoJS.enc.Base64.parse(key);
    const encrypted = CryptoJS.AES.encrypt(JSON.stringify(data), keyBytes, {
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7,
      iv: CryptoJS.lib.WordArray.random(16) 
    });

    const iv = encrypted.iv.toString(CryptoJS.enc.Base64);
    const ciphertext = encrypted.ciphertext.toString(CryptoJS.enc.Base64);
    return `${iv}:${ciphertext}`;
  }
}